using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stat for keeping track of hydration levels of refugees.
/// </summary>
public class Hydration : Int_Stat
{
    private bool isFullyHydrated = true;
    private bool isDehydrated = false;

    /// <summary>
    /// Sets whether or not the stat is fully hydrated, dehydrated, or is in between.
    /// </summary>
    protected override void CurrStatChanged()
    {
        if(currStatVal <= minStatVal)
        {
            isDehydrated = true;
        }
        else if(currStatVal >= maxStatVal)
        {
            isFullyHydrated = true;
            isDehydrated = false;
        }
        else
        {
            isFullyHydrated = false;
            isDehydrated = false;
        }
    }
}
