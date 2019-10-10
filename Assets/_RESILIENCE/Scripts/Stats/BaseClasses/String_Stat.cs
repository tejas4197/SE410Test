using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains data for a single string stat.
/// </summary>
public abstract class String_Stat : Stat
{
    [SerializeField] private string defaultStatValue;

    protected string currStatVal;

    #region Getters_Setters

    /// <summary>
    /// Returns the current stat val.
    /// </summary>
    /// <returns>Returns the current stat val.</returns>
    public string GetCurrStatVal()
    {
        return currStatVal;
    }

    protected abstract void CurrStatChanged();

    #endregion

    #region Monobehaviour
    private void Start()
    {
        currStatVal = defaultStatValue;
    }
    #endregion
}
