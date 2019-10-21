using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stat for tracking the Food levels.
/// </summary>
public class Food : Int_Stat
{
    #region ATTRIBUTES

    private bool isFull = true;
    private bool isStarving = false;

    #endregion

    #region     GETTERS_SETTERS

    /// <summary>
    /// If the currValue is at the maximum value.
    /// </summary>
    /// <returns>Returns if the currValue is at the maximum level.</returns>
    public bool GetIsFull()
    {
        return isFull;
    }

    /// <summary>
    /// If the currValue is at the minimum value.
    /// </summary>
    /// <returns>Returns if the currValue is at the minimum level.</returns>
    public bool GetIsStarving()
    {
        return isStarving;
    }

    #endregion


    /// <summary>
    /// Sets whether or not the stat is fully hydrated, dehydrated, or is in between.
    /// </summary>
    protected override void CurrStatChanged()
    {
        if (currStatVal <= minStatVal)
        {
            isStarving = true;
        }
        else if (currStatVal >= maxStatVal)
        {
            isFull = true;
            isStarving = false;
        }
        else
        {
            isFull = false;
            isStarving = false;
        }
    }
}
