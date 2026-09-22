using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Vibing
{
    /// <summary>
    /// Manages clicker game logic, scoring, UI updates, and interactions with the target Square object.
    /// </summary>
    public class ClickerManager : MonoBehaviour
    {
        public static ClickerManager Instance { get; private set; }

        [Header("Target Square")]
        [Tooltip("The square GameObject to click on.")]
        [SerializeField] private GameObject targetSquare;
        [SerializeField] private Transform squareTransform;
        [SerializeField] private SpriteRenderer squareRenderer;
        [SerializeField] private Collider2D squareCollider;
        [SerializeField] private SquareRotator squareRotator;

        [Header("Clicker Data")]
        [SerializeField] private long score = 0;
        [SerializeField] private int pointsPerClick = 1;

        [Header("UI References (TextMeshPro)")]
        [Tooltip("Displays the current score / click count.")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [Tooltip("Displays current points gained per click.")]
        [SerializeField] private TextMeshProUGUI perClickText;

        [Header("Visual Feedback (Bounce)")]
        [SerializeField] private bool enableBounceEffect = true;
        [SerializeField] private Vector3 punchScale = new Vector3(1.2f, 1.2f, 1f);
        [SerializeField] private float bounceDuration = 0.12f;

        [Header("Upgrades & Effects")]
        [SerializeField] private bool hasParticleEffect = false;
        [SerializeField] private ParticleSystem clickParticles;
        [SerializeField] private bool hasRandomColor = false;

        [Header("Audio Feedback (Optional)")]
        [SerializeField] private AudioClip clickSound;
        private AudioSource audioSource;

        [Header("Events")]
        public UnityEvent<long> onScoreChanged;
        public UnityEvent onSquareClicked;

        private Vector3 originalScale = Vector3.one;
        private Coroutine bounceCoroutine;
        private Camera mainCamera;

        public long Score => score;
        public int PointsPerClick
        {
            get => pointsPerClick;
            set
            {
                pointsPerClick = Mathf.Max(1, value);
                UpdateUI();
            }
        }

        public bool HasParticleEffect
        {
            get => hasParticleEffect;
            set => hasParticleEffect = value;
        }

        public ParticleSystem ClickParticles
        {
            get => clickParticles;
            set => clickParticles = value;
        }

        public bool HasRandomColor
        {
            get => hasRandomColor;
            set => hasRandomColor = value;
        }

        public SpriteRenderer SquareRenderer => squareRenderer;
        public GameObject TargetSquare => targetSquare;
        public SquareRotator SquareRotator => squareRotator;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            mainCamera = Camera.main;

            if (targetSquare == null)
            {
                targetSquare = GameObject.Find("Square");
            }

            if (targetSquare != null)
            {
                if (squareTransform == null) squareTransform = targetSquare.transform;
                if (squareRenderer == null) squareRenderer = targetSquare.GetComponent<SpriteRenderer>();
                if (squareCollider == null) squareCollider = targetSquare.GetComponent<Collider2D>();
                if (squareRotator == null) squareRotator = targetSquare.GetComponent<SquareRotator>();
                originalScale = squareTransform.localScale;
            }

            if (clickSound != null)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void Start()
        {
            UpdateUI();
        }

        private void Update()
        {
            DetectClickInput();
        }

        private void DetectClickInput()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                if (mainCamera == null) return;
            }

            // Do not click square if pointer is over UI elements
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

#if ENABLE_INPUT_SYSTEM
            var pointer = Pointer.current;
            if (pointer != null && pointer.press.wasPressedThisFrame)
            {
                Vector2 screenPos = pointer.position.ReadValue();
                CheckClickAtPosition(screenPos);
            }
#else
            if (Input.GetMouseButtonDown(0))
            {
                CheckClickAtPosition(Input.mousePosition);
            }
#endif
        }

        private void CheckClickAtPosition(Vector2 screenPosition)
        {
            Vector3 worldPoint = mainCamera.ScreenToWorldPoint(screenPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null && (hit.collider == squareCollider || hit.collider.gameObject == targetSquare))
            {
                OnSquareClicked();
            }
        }

        /// <summary>
        /// Triggered when the square is clicked or tapped.
        /// </summary>
        public void OnSquareClicked()
        {
            AddScore(pointsPerClick);

            // Bounce effect
            if (enableBounceEffect && squareTransform != null)
            {
                if (bounceCoroutine != null) StopCoroutine(bounceCoroutine);
                bounceCoroutine = StartCoroutine(BounceRoutine());
            }

            // Spin boost effect
            if (squareRotator != null)
            {
                squareRotator.AddSpinBoost();
            }

            // Random color changer effect
            if (hasRandomColor && squareRenderer != null)
            {
                squareRenderer.color = Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.9f, 1f);
            }

            // Click particle effect
            if (hasParticleEffect && clickParticles != null)
            {
                clickParticles.Emit(18);
            }

            // Sound
            if (audioSource != null && clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }

            onSquareClicked?.Invoke();
        }

        public void AddScore(long amount)
        {
            score += amount;
            UpdateUI();
            onScoreChanged?.Invoke(score);
        }

        /// <summary>
        /// Spends score currency if the player has enough clicks. Returns true if successful.
        /// </summary>
        public bool TrySpendScore(long amount)
        {
            if (score >= amount)
            {
                score -= amount;
                UpdateUI();
                onScoreChanged?.Invoke(score);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates the TextMeshPro UI elements with current values.
        /// </summary>
        public void UpdateUI()
        {
            if (scoreText != null)
            {
                scoreText.text = $"Clicks: {score:N0}";
            }
            if (perClickText != null)
            {
                perClickText.text = $"+{pointsPerClick:N0} per click";
            }
        }

        private IEnumerator BounceRoutine()
        {
            float elapsed = 0f;
            float halfDuration = bounceDuration / 2f;

            // Squash/punch up
            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / halfDuration;
                squareTransform.localScale = Vector3.Lerp(originalScale, punchScale, t);
                yield return null;
            }

            // Return back to original
            elapsed = 0f;
            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / halfDuration;
                squareTransform.localScale = Vector3.Lerp(punchScale, originalScale, t);
                yield return null;
            }

            squareTransform.localScale = originalScale;
            bounceCoroutine = null;
        }
    }
}
