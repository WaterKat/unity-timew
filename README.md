# TimeW

A small Unity package for clocks, delayed tasks and per-frame timed tasks, written in 2019.

> **Archived.** This project is no longer maintained. It was last worked on in December 2019 and is kept as a historical snapshot, with a handful of bug fixes added during archival (see [CHANGELOG.md](CHANGELOG.md)). Those fixes were **not compiled or tested inside Unity**, because no Unity editor was available when they were made. Treat the code as reference material, not production-ready.

- Package: `com.waterkat.timew`, version 1.0.2
- Namespace: `WaterKat.TimeW`
- Unity: written for 2018.1 and later (`package.json`)
- Original, unfixed state: commit `6dc246e` (version 1.0.1, 2019-12-07)

## What's in it

| Class | Purpose |
| --- | --- |
| `ClockManager` | Singleton that creates and tracks clocks, each with a start and end time, identified by an integer ID |
| `TaskManager` | Runs an `Action` once after a delay |
| `LoopedTaskManager` | Calls an `Action<double>` every frame with a progress value from 0 to 1, then removes the task when its time is up |
| `Ticker` | Helper that returns `true` once every `MaxTick` seconds when you call `Tick()` each frame |
| `TimeW.prefab` | Prefab with the three managers as children, which you can drop into a scene |

The managers create themselves on first use, so the prefab is optional.

## Install

Copy this folder into your project's `Assets` or `Packages` folder. In recent Unity versions you can instead add a git dependency to `Packages/manifest.json`:

```json
"com.waterkat.timew": "https://github.com/WaterKat/TimeW.git"
```

## Usage

```csharp
using UnityEngine;
using WaterKat.TimeW;

public class Example : MonoBehaviour
{
    Ticker ticker = new Ticker(0.5f);   // every half second

    void Start()
    {
        // Run once after 2 seconds
        int taskID = TaskManager.AddTask(() => Debug.Log("Two seconds passed"), 2.0);
        // TaskManager.RemoveTask(taskID);

        // Call every frame for 3 seconds with progress 0..1, then stop
        LoopedTaskManager.AddLoopedTask(progress => Debug.Log(progress), 3.0);

        // Same, but wait 1 second before starting
        LoopedTaskManager.AddDelayedLoopedTask(1.0, progress => Debug.Log(progress), 3.0);

        // Use a clock directly
        int clockID = ClockManager.AddClock(5.0);
        // ClockManager.ClockMet(clockID), ClockManager.ClockRelativeTimeElapsed(clockID), ...
    }

    void Update()
    {
        if (ticker.Tick()) Debug.Log("Tick");
    }
}
```

`ClockManager` also offers `AddDelayedClock`, `OverrideClock`, `RemoveClock`, `ClockStarted`, `ClockTimeElapsed`, and the static `deltaTicks`, `deltaSeconds` and `deltaMinutes`.

## Known limitations (not fixed)

- **Wall-clock time.** Clocks are based on `DateTime.Now`, measured from when `ClockManager` woke up. They ignore `Time.timeScale` and pausing, and they are affected by changes to the system clock. Despite its name, `deltaSeconds` is the time since startup, not the time since the last frame.
- **State is not persistent.** The managers do not use `DontDestroyOnLoad`, so clocks and tasks are lost when a scene unloads. If a second instance of a manager is created, it destroys the older one and everything that instance was tracking.
- **Task errors are vague.** If a task throws, the manager logs a generic error without the exception details and carries on.
- **Task IDs are random integers.** `0` is reserved and never issued.
- **Legacy code is included.** `PreviousVersions/TimerManager.cs` is an older implementation (namespace `WaterKat.OLD_Time`). Nothing in the package uses it, but it is compiled with the package.
- **No tests and no license file.**
