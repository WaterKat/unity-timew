using UnityEngine;

namespace WaterKat.TimeW
{
    /// <summary>
    /// Repeating interval helper. Call Tick() every frame; it returns true once
    /// every MaxTick seconds.
    /// </summary>
    [System.Serializable]
    public class Ticker
    {
        public float MaxTick = 1;   //in Seconds

        private double lastTickTime;    //in Seconds, on the ClockManager timeline
        private bool started;

        public bool Tick()
        {
            double now = ClockManager.deltaSeconds;
            if (!started)
            {
                started = true;
                lastTickTime = now;
                return false;
            }
            if (now - lastTickTime >= MaxTick)
            {
                lastTickTime = now;
                return true;
            }
            return false;
        }
        public void Reset()
        {
            started = false;
        }

        public Ticker() { }
        public Ticker(float desiredTickTime)
        {
            MaxTick = desiredTickTime;
        }
    }
}
