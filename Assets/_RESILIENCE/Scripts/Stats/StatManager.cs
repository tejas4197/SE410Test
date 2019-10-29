using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : Singleton<StatManager>
{
    /// <summary>
    /// Value that if equal or above, is said to be doing good.
    /// </summary>
    [SerializeField] private int goodStatValue = 70;

    /// <summary>
    /// Value that if equal or below, is said to be doing bad.
    /// </summary>
    [SerializeField] private int badStatValue = 30;

    #region GETTERS_SETTERS
    /// <summary>
    /// Returns good stat value
    /// </summary>
    /// <returns>goosStatValue</returns>
    public int GetGoodStatValue()
    {
        return goodStatValue;
    }

    /// <summary>
    /// Returns badStatValue
    /// </summary>
    /// <returns>badStatValue</returns>
    public int GetBadStatValue()
    {
        return badStatValue;
    }
    #endregion
}
