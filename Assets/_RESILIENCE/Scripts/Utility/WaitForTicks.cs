using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForTicks : CustomYieldInstruction
{
    int ticks;
    
    public override bool keepWaiting
    {
        get
        {
            return (ticks > 0);
        }
    }

    public WaitForTicks(int _ticks)
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
        //Debug.Log(ticks); // when this is commented out the behaviour changes?????
        ticks--;
    }

}
