using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForTicks : CustomYieldInstruction
{
    float ticks;
    
    public override bool keepWaiting
    {
        get
        {
            return (ticks > 0);
        }
    }

    public WaitForTicks(float _ticks)
    {
        ticks = _ticks;
        TimeManager.GetInstance().OnTick += ApplyTick;
    }

    ~WaitForTicks()
    {
        TimeManager.GetInstance().OnTick -= ApplyTick;
    }

    public void ApplyTick()
    {
        ticks -= TimeManager.GetInstance().GetDeltaTime();
    }

}
