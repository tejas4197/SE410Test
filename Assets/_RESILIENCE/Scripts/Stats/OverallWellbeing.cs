using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallWellbeing : Int_Stat
{
    /// <summary>
    /// Does the math for the curr stat value while clamping to the min and max possible.
    /// </summary>
    /// <param name="_health">Health value</param>
    /// <param name="_water">Hydration value</param>
    public void SetCurrStat(int _health, int _water)
    {
        int val = Mathf.RoundToInt((_health + _water) / 2); // currently doing the mean. going to tweak this/do more nuanced math later
        currStatVal = Mathf.Clamp(val, minStatVal, maxStatVal);
        CurrStatChanged();
    }

    protected override void CurrStatChanged()
    {
        // ...?
    }
}
