# Vibing Click

> An experimental Unity project exploring **agentic AI-assisted game development** through Unity MCP, Unity AI, Unity CLI, and AI agent clients.

**Vibing Click** is a research and prototyping project focused on exploring how far an agentic AI workflow can assist with building a small Unity game from development tasks to implementation.

The prototype's target genre is an **idle clicker**, chosen because its relatively simple gameplay loop provides a good environment for experimenting with AI-driven development workflows.

## 🎯 Project Goals

The primary goal is **not** to build a production-ready game, but to experiment with different AI-assisted development approaches within a Unity project.

Areas being explored:

* AI-driven Unity development
* Agentic workflows for game development
* Unity MCP integration
* Unity AI capabilities
* Unity CLI integration
* AI coding agents / agentic clients
* Automated Unity editor interactions
* Generating and modifying game systems through AI
* Evaluating how well AI agents understand an existing Unity codebase
* Exploring repeatable AI-assisted development workflows

## 🕹️ Prototype

**Genre:** Idle Clicker

The prototype will serve as a playground for testing agentic workflows.

Potential systems may include:

* Clicking / resource generation
* Resource accumulation
* Passive income
* Simple UI
* Basic game feedback
* Automated balancing experiments

The exact feature set is intentionally flexible and may change throughout the research.

## 🤖 AI / Agentic Stack

This repository is intended to experiment with multiple AI development tools and compare their workflows.

### Unity

* Unity Editor
* Unity MCP
* Unity AI
* Unity CLI

### Agentic AI Clients

Potential clients/tools will be evaluated based on:

* Unity integration
* MCP support
* Code generation quality
* Ability to inspect and modify project files
* Ability to interact with the Unity Editor
* Ability to execute development workflows
* Context management
* Reliability
* Ease of iteration

> The exact tools used will evolve as the research progresses.

## 🧪 Research Areas

### 1. Unity MCP

Exploring how Model Context Protocol can expose Unity functionality to an AI agent.

Questions being explored:

* What Unity functionality can an agent control?
* How well can an agent understand the current Unity scene?
* Can an agent create and modify GameObjects?
* Can it configure components and assets?
* Can it iterate based on runtime/editor feedback?

### 2. Unity AI

Exploring Unity's native AI capabilities and how they can complement external agentic workflows.

### 3. Unity CLI

Exploring whether Unity CLI can become part of an autonomous development loop.

Potential workflow:

```text
AI Agent
   ↓
Modify Project
   ↓
Unity CLI
   ↓
Build / Test
   ↓
Collect Result
   ↓
AI Agent
   ↓
Analyze / Fix
   ↺
```

### 4. Agentic Development Workflow

The main experiment is determining whether an AI agent can perform development tasks beyond simply generating code.

For example:

```text
Task
 ↓
Understand Project
 ↓
Inspect Existing Code
 ↓
Plan Changes
 ↓
Modify Code / Assets
 ↓
Run Unity
 ↓
Test
 ↓
Inspect Result
 ↓
Fix / Iterate
```

## 📁 Repository Structure

The project structure will evolve alongside the experiments.

```text
Vibing-Click/
├── Assets/
│   ├── Scripts/
│   ├── Scenes/
│   ├── Prefabs/
│   └── ...
│
├── Packages/
├── ProjectSettings/
│
├── Documentation/
│   └── Research/
│
├── .gitignore
└── README.md
```

## 🔬 Experiment Log

Experiments and findings will be documented as the project develops.

| Experiment                 | Status       | Notes |
| -------------------------- | ------------ | ----- |
| Unity MCP                  | 🧪 Exploring |       |
| Unity AI                   | 🧪 Exploring |       |
| Unity CLI                  | 🧪 Exploring |       |
| AI Agent Client            | 🧪 Exploring |       |
| Agent-driven coding        | 🧪 Exploring |       |
| Agent-driven Unity editing | 🧪 Exploring |       |
| Automated testing loop     | 🧪 Exploring |       |
| Autonomous iteration       | 🧪 Exploring |       |

## 📌 Philosophy

This project is intentionally experimental.

The objective is to discover:

> **How much of a Unity game development workflow can be delegated to an AI agent while still keeping the development process understandable, controllable, and maintainable?**

The prototype itself is secondary to the research.

Failures, limitations, unexpected behavior, and inefficient workflows are considered valuable results of the experiment.

## 🚧 Status

**Research / Prototype — Work in Progress**

Expect frequent changes to both the game and the development workflow.

---

**Project:** Vibing Click
**Purpose:** Agentic AI × Unity Research
**Genre:** Idle Clicker
