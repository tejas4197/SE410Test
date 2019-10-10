using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains a boolean stat.
/// </summary>
public abstract class Bool_Stat : Stat
{
    [SerializeField] private  bool defaultStatValue;

    protected bool currStatVal;

    #region Getters_Setters

    /// <summary>
    /// Returns the current stat val.
    /// </summary>
    /// <returns>Returns the current stat val.</returns>
    public bool GetCurrStatVal()
    {
        return currStatVal;
    }

    /// <summary>
    /// Function that is called whenever the current stat value is changed.
    /// </summary>
    protected abstract void CurrStatChanged();

    #endregion

    #region Monobehaviour
    /// <summary>
    /// Sets currStatVal equal to the default value.
    /// </summary>
    private void Start()
    {
        currStatVal = defaultStatValue;
    }
    #endregion
}
