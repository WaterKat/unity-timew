using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterKat.TimeW
{
    [System.Serializable]
    class Ticker
    {
        float currentTick = 0;      //in Seconds
        public float MaxTick = 1;   //in Seconds

        public bool Tick()
        {
            if (currentTick > MaxTick) { currentTick = 0; }
            currentTick += (float)ClockManager.deltaSeconds;
            return currentTick >= MaxTick;
        }
        public void Reset()
        {
            currentTick = 0;
        }

        public Ticker() { }
        public Ticker(float desiredTickTime)
        {
            MaxTick = desiredTickTime;
        }
    }
}