using UnityEngine;

namespace Vibing
{
    /// <summary>
    /// Continuously rotates the GameObject with configurable speed and direction,
    /// with an optional click spin boost for extra game juice.
    /// </summary>
    public class SquareRotator : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [Tooltip("Base rotation speed in degrees per second.")]
        [SerializeField] private float rotationSpeed = 45f;

        [Tooltip("If true, rotates clockwise; if false, rotates counter-clockwise.")]
        [SerializeField] private bool clockwise = false;

        [Tooltip("Toggle rotation on or off.")]
        [SerializeField] private bool isRotating = true;

        [Header("Click Interaction (Juice)")]
        [Tooltip("Gives an extra burst of spin speed whenever clicked.")]
        [SerializeField] private bool boostOnClick = true;

        [Tooltip("Additional degrees per second added on click.")]
        [SerializeField] private float clickBoostAmount = 180f;

        [Tooltip("How quickly the click spin boost decays back to base speed.")]
        [SerializeField] private float boostDecayRate = 250f;

        private float currentBoost = 0f;

        public float RotationSpeed
        {
            get => rotationSpeed;
            set => rotationSpeed = value;
        }

        public bool Clockwise
        {
            get => clockwise;
            set => clockwise = value;
        }

        public bool IsRotating
        {
            get => isRotating;
            set => isRotating = value;
        }

        private void Update()
        {
            if (!isRotating) return;

            // Decay boost smoothly back to 0
            if (currentBoost > 0f)
            {
                currentBoost = Mathf.MoveTowards(currentBoost, 0f, boostDecayRate * Time.deltaTime);
            }

            float effectiveSpeed = rotationSpeed + currentBoost;
            float direction = clockwise ? -1f : 1f;
            float angleDelta = direction * effectiveSpeed * Time.deltaTime;

            transform.Rotate(0f, 0f, angleDelta);
        }

        /// <summary>
        /// Adds a momentary burst to the rotation speed.
        /// </summary>
        public void AddSpinBoost(float boost = -1f)
        {
            if (!boostOnClick) return;
            currentBoost += (boost > 0f ? boost : clickBoostAmount);
        }

        /// <summary>
        /// Parameterless helper for UnityEvents / Inspector callbacks.
        /// </summary>
        public void TriggerSpinBoost()
        {
            AddSpinBoost(-1f);
        }
    }
}
