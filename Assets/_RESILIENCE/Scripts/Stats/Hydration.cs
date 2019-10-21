using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stat for keeping track of hydration levels of refugees.
/// </summary>
public class Hydration : Int_Stat
{
    #region ATTRIBUTES

    private bool isFullyHydrated = true;
    private bool isDehydrated = false;

    #endregion

    #region GETTERS_SETTERS

    /// <summary>
    /// Tells whether the currStat Value is at maximum.
    /// </summary>
    /// <returns>Returns if the hydration stat is at maximum.</returns>
    public bool GetFullyHydrated()
    {
        return isFullyHydrated;
    }

    /// <summary>
    /// Tells whether the currStat Value is at the minimum.
    /// </summary>
    /// <returns>Returns if the hydration stat is at minimum.</returns>
    public bool GetDehydrated()
    {
        return isDehydrated;
    }

    #endregion

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
