# Match-3 Puzzle

🚧 **Work In Progress** · A mobile-style match-3 puzzle game built in Unity 6, in the spirit of *Royal Match* — swap-to-match, cascading drops, and chain-reaction combos.

> **Status — In active development.** The core board loop (board generation → swap → match detection → clear → collapse → refill → re-scan) is implemented and playable. UI, level progression, and meta systems are still being built out. Expect placeholder art and temporary screens while the project matures.

---

## About This Project

This is a solo, in-progress portfolio project demonstrating production-style Unity game architecture. The goal isn't just a working match-3 — it's a codebase organized the way a shippable mobile title would be: modular systems, designer-friendly tooling, and patterns that scale past a prototype.

It's built to show how I approach a real game project — not only *that* the game works, but *how* it's put together under the hood.

## What This Project Demonstrates

For reviewers without a coding background, here's what the engineering work shows — each point is verifiable in the source by a technical reviewer:

- **Clean, modular architecture** — the game is split into self-contained systems, each in its own folder and namespace, so features can grow without tangling together.
- **Designer-first workflow** — gameplay values (board size, animation timing, level layouts) are tuned visually in the Unity Editor rather than hard-coded, so the game can be balanced without touching code.
- **Custom editor tooling** — purpose-built Unity Inspector tools for authoring levels and managing save data, the kind of quality-of-life work a senior developer builds for a team.
- **Performance-minded patterns** — object pooling and asynchronous (non-blocking) game flow keep the game smooth on mobile hardware.
- **Robust, encrypted save system** — player progress is persisted safely with a reusable, encrypted save framework.

## Tech Stack

| Area | Choice |
|------|--------|
| Engine | Unity **6000.3.8f1** (Unity 6) |
| Render pipeline | Universal Render Pipeline (URP) 17.3.0 |
| Language | C# (.NET Standard 2.1) |
| Input | Unity Input System 1.18.0 |
| Async | UniTask (allocation-free async/await) |
| Animation | DOTween (tweening) |
| UI | Unity UI (uGUI) |

## Gameplay & Screenshots

<div align="center">
<video src=https://github.com/user-attachments/assets/6a5823a8-3af5-4992-8c6b-d1cd1b88118b></video>
</div>

## Architecture Overview

The project is organized into **self-contained systems**, each living under `Assets/_Project/<System>/` with its own `Match3.*` namespace. Systems communicate through ScriptableObject references rather than hard dependencies, keeping each piece independently understandable.

```
Assets/_Project/
├── Core/            Match3.Core           — the match-3 board engine (largest system)
│   ├── GameBoard/     grid construction, cells, board config
│   ├── BoardItems/    pieces, per-piece data, pooled spawner
│   ├── BoardResolver  match → clear → collapse → refill → re-scan loop
│   ├── MatchScanner   non-destructive match detection
│   └── Helpers/       array / list / task extension utilities
├── GameZone/        Match3.GameZone       — play-zone setup and framing
├── LevelSystem/     Match3.LevelSystem    — level data, level database, progression (orchestration hub)
├── ObjectiveSystem/ Match3.ObjectiveSystem— win-condition tracking + objective UI
├── PopupSystem/     Match3.PopupSystem    — animated popup orchestration (e.g. level complete)
├── SaveSystem/      Match3.SaveSystem     — encrypted, self-saving ScriptableObject persistence
├── InputSystem/     Match3.InputSystem    — wraps Unity's Input System; emits pointer events
├── ObjectPooling/   Match3.ObjectPooling  — generic, project-agnostic object pool
├── HUD/             Match3.HUD            — heads-up display (temporary placeholder, in progress)
└── UI/              shared fonts + UI prefab library
```

The project deliberately uses a single Unity assembly (no asmdefs) for fast iteration at this scale; system boundaries are kept clean by convention and by routing cross-system communication through ScriptableObject references.

## Highlighted Engineering Details

A few pieces a technical reviewer can dig into directly:

- **Generic object pool** — `ObjectPool<T>` (`Assets/_Project/ObjectPooling/`) is a reusable, type-safe pool with create / get / release callbacks and active/passive tracking, used for board pieces.
- **Encrypted save framework** — `BaseSavedScriptableObject<T>` (`Assets/_Project/SaveSystem/`) lets any data asset persist and encrypt itself, with a custom Inspector for managing save state.
- **Custom level database inspector** — `LevelDatabaseEditor` gives designers a dedicated authoring view for the level catalog instead of raw serialized fields.
- **Non-destructive board pipeline** — `MatchScanner` detects matches without mutating the grid, and `BoardResolver` runs the clear → collapse → refill → re-scan cascade, keeping detection and mutation cleanly separated.
