# Match-3 Puzzle — (**Work In Progress**)

A mobile-style match-3 puzzle game built in Unity, recreating the core board gameplay of *Royal Match*: swap-to-match, cascading drops, and chain-reaction combos.

> **Status:** In active development. The core board loop (board generation → swap → match detection → clear → collapse → refill → re-scan) is implemented and playable. UI, progression, and meta systems are in progress.

## Architecture

The project is organized into **self-contained systems**, each living under `Assets/_Project/<System>/` with its own namespace.

```
Assets/_Project/
├── Core/            Match3.Core         — board gameplay heart
│   ├── GameBoard/     grid construction, cells, board config
│   ├── BoardItems/    pieces, piece data, spawner
│   ├── BoardResolver  match → clear → collapse → refill → re-scan loop
│   ├── MatchScanner   non-destructive match detection
│   └── Helpers/       array / task / general extensions
├── InputSystem/     Match3.InputSystem  — wraps Unity's Input System; emits PointerDown/PointerUp events
├── LevelSystem/     Match3.LevelSystem  — level data + database SOs, custom database inspector
├── ObjectPooling/   Match3.ObjectPooling — generic, project-agnostic pool
└── HUD/             Match3.HUD          — heads-up display (placeholder, in progress)
```