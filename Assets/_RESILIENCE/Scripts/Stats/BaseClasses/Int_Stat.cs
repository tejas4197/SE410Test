using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains data for a single int stat.
/// </summary>
public abstract class Int_Stat : Stat
{
    [SerializeField] protected int minStatVal;
    [SerializeField] protected int maxStatVal;
    [SerializeField] private int defaultStatVal;

    [SerializeField] protected int currStatVal;

    #region Getters_Setters

    #region CurrStatVal

    /// <summary>
    /// Returns the current stat val.
    /// </summary>
    /// <returns>Returns the current stat val.</returns>
    public int GetCurrStatVal()
    {
        return currStatVal;
    }

    /// <summary>
    /// Adds _val to the curr stat value. Clamped to the min and max possible.
    /// </summary>
    /// <param name="_val">Value to add to the stat.</param>
    public void AddCurrStat(int _val)
    {
        if(_val < 0)
        {
            SubtractCurrStat(Mathf.Abs(_val));
            return;
        }
        if(currStatVal > maxStatVal)
        {
            Debug.LogError("Trying to add to currstat when already above max value: " + this.gameObject);
            return;
        }
        currStatVal = Mathf.Min(maxStatVal, currStatVal + _val);
        CurrStatChanged();
    }

    /// <summary>
    /// Subtracts _val to the curr stat value. Clamped to the min and max possible.
    /// </summary>
    /// <param name="_val">Value to subtract from the stat.</param>
    public void SubtractCurrStat(int _val)
    {
        if(_val < 0)
        {
            AddCurrStat(Mathf.Abs(_val));
            return;
        }
        if(currStatVal< minStatVal)
        {
            Debug.LogError("Trying to subtract to currstat when already below min value: " + this.gameObject);
            return;
        }

        currStatVal = Mathf.Max(minStatVal, currStatVal - _val);
        CurrStatChanged();
    }

    /// <summary>
    /// Sets the curr stat value while clamping to the min and max possible.
    /// </summary>
    /// <param name="_val">Value to set the stat at.</param>
    public void SetCurrStat(int _val)
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
    public int GetMinStatVal()
    {
        return minStatVal;
    }

    /// <summary>
    /// Adds to the min stat value, clamping to the min and max possible.
    /// </summary>
    /// <param name="_val"> value to add to the stat.</param>
    public void AddMinStat(int _val)
    {
        SetMinStat(minStatVal + _val);
    }

    /// <summary>
    /// Subtracts from the min stat value, clamping to the min and max possible. 
    /// </summary>
    /// <param name="_val"></param>
    public void SubtractMinStat(int _val)
    {
        SetMinStat(minStatVal - _val);
    }

    /// <summary>
    /// Sets the min stat value, clamping to the min, max possible.
    /// </summary>
    /// <param name="_val">Value to set the min stat to.</param>
    public void SetMinStat(int _val)
    {
        minStatVal = Mathf.Min(_val, maxStatVal);
    }

    #endregion

    #region MaxStatVal

    /// <summary>
    /// Gets the max stat value.
    /// </summary>
    /// <returns>The max stat value.</returns>
    public int GetMaxStatVal()
    {
        return maxStatVal;
    }

    /// <summary>
    /// Adds to the max stat value, while clamping to the min, max possible.
    /// </summary>
    /// <param name="_val">value to add to the max stat.</param>
    public void AddMaxStat(int _val)
    {
        SetMaxStat(maxStatVal + _val);
    }

    /// <summary>
    /// Subtracts from the max stat value, while clamping to the min possibe.
    /// </summary>
    /// <param name="_val">Value to subtract from the max stat.</param>
    public void SubtractMaxStat(int _val)
    {
        SetMaxStat(maxStatVal - _val);
    }

    /// <summary>
    /// Sets the max stat value, while clamping above the min possible.
    /// </summary>
    /// <param name="_val"></param>
    public void SetMaxStat(int _val)
    {
        maxStatVal = Mathf.Max(_val, minStatVal);
    }

    #endregion

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        currStatVal = defaultStatVal;
    }

    #endregion
}
