# Changelog

## 1.0.2 - archival fixes

Made while archiving the project. None of these changes were compiled or run in Unity, because no Unity editor was available. They were checked by reading the code, validating the prefab's GUIDs against the script `.meta` files, confirming the braces balance, and comparing the old and new `Ticker` logic in a small simulation.

### Fixed

- **`TimeW.prefab`: broken script references.** The three script components pointed at GUIDs that match no script in this repository (`1ef9f8cf...`, `ac2d35fa...`, `918c5fad...`), so Unity would show them as missing scripts. They now point at the real GUIDs from the scripts' `.meta` files: `ClockManager` -> `ba195612...`, `LoopedTaskManager` -> `4ac84b50...`, `TaskManager` -> `a4209b32...`.
- **`Ticker`: fired far too often.** `Tick()` added `ClockManager.deltaSeconds` to its counter every call. That value is the total time since startup, not the time since the last frame, so the counter ran away and the ticker fired almost every frame. It now remembers the time of its last tick and returns `true` only when `MaxTick` seconds have passed since then. In a 10-second simulation at 60 fps with a 1-second interval, the old logic fired 562 times and the new logic fires 9 times. The first call to `Tick()` after construction or `Reset()` starts the interval and returns `false`.
- **`Ticker` was not usable from other code.** The class had no access modifier, which makes it `internal` to the package. It is now `public`.
- **`TaskManager.RemoveTasksWithClock` and `LoopedTaskManager.RemoveLoopedTasksWithClock`: crash on removal.** Both removed entries from a dictionary while looping over it, which throws an `InvalidOperationException` as soon as a task matched. They now collect the matching IDs first and remove them afterwards.
- **`ClockManager.ClockRelativeTimeElapsed`: bad progress values.** It divided by the clock's duration, so a zero-length clock produced `NaN` or infinity, and the value was not limited to the 0 to 1 range. The last call for a looped task could therefore receive a number above 1. It now returns 1 once a zero-length clock has ended, 0 before that, and otherwise clamps progress to between 0 and 1. This is a small behavior change: the final progress value a looped task receives is now exactly 1.
- **`package.json`: broken documentation link and typo.** The description told users to click "View Documentation", but no `documentationUrl` was set. It now points at this repository's README. The description's "manager time" typo is now "manage time". The version is now 1.0.2.

### Added

- `README.md` with the archive notice, install steps, usage examples and known limitations.
- This changelog.

### Not changed

The limitations listed in the README are deliberate non-fixes: the wall-clock time base, the non-persistent singletons, the generic task error message, and the unused legacy `PreviousVersions/TimerManager.cs`.

## 1.0.1 - 2019-12-07

The original, unmodified project (commit `6dc246e`).
