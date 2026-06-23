# Middle Unity Developer Test — Octo Games

## 1) Coding Principles

1) Separation of responsibilities  
Gameplay exposes state/events. UI renders. No per-frame scene searches.

2) Performance hygiene  
No `GetComponent/Find` in update loops, no per-frame allocations/log spam. UI updates on interval or on change.

## 2) Save / Load Utility

Code: `Assets/Scripts/TestTask/Infrastructure/SaveLoad/SaveUtility.cs`

- Generic save/load by key \(T\)
- Missing/invalid data handled safely (returns false)
- Atomic write (temp + replace/move)
- One small utility (no extra layers)

## 3) Popup / UI System

Code: `Assets/Scripts/TestTask/UI/Popups/*`

- Loads popup prefab (default: `Resources`)
- Title + body
- 1–5 buttons with callbacks

### 3.1) Popup prefab components

`Canvas`, `GraphicRaycaster`, `CanvasGroup`, `Image`, `TMP_Text`, `Button`, layout: `VerticalLayoutGroup/HorizontalLayoutGroup`, optional `ContentSizeFitter/LayoutElement`.

## 4) UI Refactoring

Code: `Assets/Scripts/TestTask/UI/Characters/CharactersView.cs`

- Fix avg logic
- Cache references
- Update by interval (not FixedUpdate)
- Avoid allocations and repeated queries

## 5) Gameplay entity tracking

Code: `Assets/Scripts/TestTask/Gameplay/EntityRegistry/*`

- Register/unregister on enable/disable/destroy
- Return only active entities

## Bonus (not required)

### How to scale these systems

On a small test like this, everything can live in one place, but on a real project I'd start with simple feature separation and asmdefs: gameplay shouldn't depend on UI, and UI shouldn't touch saves directly. Save/Load usually hits versioning first — you add a `version` field to the data and write migrations, because after the first release the format almost always changes. For popups on larger projects, a single instance or a small pool is usually enough, plus a queue if popups can stack (confirm on top of a tutorial, etc.)

### How designers would interact with this code

Designers need to change visuals and text without touching code. For popups I usually provide prefab templates for different cases (confirm, warning, choice) and configurable styles via ScriptableObjects — colors, buttons, animations. Code only calls `Show` with data, not layout work. Text is better kept as localization keys, especially on multilingual projects — you pass a key in the request and the UI pulls the string. For `CharactersView`, a designer or level designer just drops Transform references in the inspector and tweaks the update rate — that's enough to tune the UI for a scene without a programmer.

### How to profile / debug performance issues

If the UI starts lagging, I open the Profiler and check GC Alloc first — on mobile that's often the first red flag. Then I look for spikes when the UI updates (in our case, when refresh runs). Usual suspects: `string.Format`/concatenation every frame, `Debug.Log` in Update, `GetComponent`/`Find` inside loops. If it's unclear where the cost is coming from, I temporarily put a timer around refresh and compare: every frame vs once every N frames. Usually the difference shows up right away in the Profiler and on device.

