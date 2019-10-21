using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stat for keeping track of hydration levels.
/// </summary>
public class Hygiene : Int_Stat
{
    #region ATTRIBUTES

    private bool isBest = true;
    private bool isWorst = false;

    #endregion

    #region GETTERS_SETTERS

    /// <summary>
    /// Tells whether the currStat value is at maximum.
    /// </summary>
    /// <returns>Returns bool if the currStat is at maximum.</returns>
    public bool GetIsBest()
    {
        return isBest;
    }

    /// <summary>
    /// Tells whether the currStat value is at minimum.
    /// </summary>
    /// <returns>Returns bool if the currStat is at minimum.</returns>
    public bool GetIsWorst()
    {
        return isWorst;
    }

    #endregion

    /// <summary>
    /// Sets whether or not the stat is fully hydrated, dehydrated, or is in between.
    /// </summary>
    protected override void CurrStatChanged()
    {
        if (currStatVal <= minStatVal)
        {
            isWorst = true;
        }
        else if (currStatVal >= maxStatVal)
        {
            isBest = true;
            isWorst = false;
        }
        else
        {
            isBest = false;
            isWorst = false;
        }
    }
}
