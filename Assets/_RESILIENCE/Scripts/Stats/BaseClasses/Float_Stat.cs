using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains data for a single float stat.
/// </summary>
public abstract class Float_Stat : Stat
{
    [SerializeField] protected float minStatVal;
    [SerializeField] protected float maxStatVal;
    [SerializeField] private float defaultStatVal;

    protected float currStatVal;

    #region Getters_Setters

    #region CurrStatVal

    /// <summary>
    /// Returns the current stat val.
    /// </summary>
    /// <returns>Returns the current stat val.</returns>
    public float GetCurrStatVal()
    {
        return currStatVal;
    }

    /// <summary>
    /// Adds _val to the curr stat value. Clamped to the min and max possible.
    /// </summary>
    /// <param name="_val">Value to add to the stat.</param>
    public void AddCurrStat(float _val)
    {
        SetCurrStat(currStatVal + _val);
        CurrStatChanged();
    }

    /// <summary>
    /// Subtracts _val to the curr stat value. Clamped to the min and max possible.
    /// </summary>
    /// <param name="_val">Value to subtract from the stat.</param>
    public void SubtractCurrStat(float _val)
    {
        SetCurrStat(currStatVal - _val);
        CurrStatChanged();
    }

    /// <summary>
    /// Sets the curr stat value while clamping to the min and max possible.
    /// </summary>
    /// <param name="_val">Value to set the stat at.</param>
    public void SetCurrStat(float _val)
    {
        currStatVal = Mathf.Clamp(_val, minStatVal, maxStatVal);
        CurrStatChanged();
    }

    protected abstract void CurrStatChanged();

    #endregion

    #region MinStatVal

    /// <summary>
    /// Returns the min stat val.
    /// </summary>
    /// <returns>Returns the min stat val.</returns>
    public float GetMinStatVal()
    {
        return minStatVal;
    }

    /// <summary>
    /// Adds to the min stat value, clamping to the min and max possible.
    /// </summary>
    /// <param name="_val"> value to add to the stat.</param>
    public void AddMinStat(float _val)
    {
        SetMinStat(minStatVal + _val);
    }

    /// <summary>
    /// Subtracts from the min stat value, clamping to the min and max possible. 
    /// </summary>
    /// <param name="_val"></param>
    public void SubtractMinStat(float _val)
    {
        SetMinStat(minStatVal - _val);
    }

    /// <summary>
    /// Sets the min stat value, clamping to the min, max possible.
    /// </summary>
    /// <param name="_val">Value to set the min stat to.</param>
    public void SetMinStat(float _val)
    {
        minStatVal = Mathf.Min(_val, maxStatVal);
    }

    #endregion

    #region MaxStatVal

    /// <summary>
    /// Gets the max stat value.
    /// </summary>
    /// <returns>The max stat value.</returns>
    public float GetMaxStatVal()
    {
        return maxStatVal;
    }

    /// <summary>
    /// Adds to the max stat value, while clamping to the min, max possible.
    /// </summary>
    /// <param name="_val">value to add to the max stat.</param>
    public void AddMaxStat(float _val)
    {
        SetMaxStat(maxStatVal + _val);
    }

    /// <summary>
    /// Subtracts from the max stat value, while clamping to the min possibe.
    /// </summary>
    /// <param name="_val">Value to subtract from the max stat.</param>
    public void SubtractMaxStat(float _val)
    {
        SetMaxStat(maxStatVal - _val);
    }

    /// <summary>
    /// Sets the max stat value, while clamping above the min possible.
    /// </summary>
    /// <param name="_val"></param>
    public void SetMaxStat(float _val)
    {
        maxStatVal = Mathf.Max(_val, minStatVal);
    }

    #endregion

    #endregion

    #region Monobehaviour

    private void Start()
    {
        currStatVal = defaultStatVal;
    }

    #endregion

}
