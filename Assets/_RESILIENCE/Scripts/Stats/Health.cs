using UnityEngine;

/// <summary>
/// Health stat, specifies more specific data pertaining to health.
/// </summary>
public class Health : Int_Stat
{
    #region Attributes
    private bool isAlive = true;
    #endregion

    #region Getters

    public bool GetIsAlive()
    {
        return isAlive;
    }
    #endregion

    #region Events
    public delegate void DeadEventHandler(object source);
    public event DeadEventHandler Died;
    #endregion


    /// <summary>
    /// Resets current health to the maximim health
    /// </summary>
    public void RestoreHealth()
    {
        currStatVal = maxStatVal;
    }

    /// <summary>
    /// When the currStatVal gets updated, check to see if this object died.
    /// </summary>
    protected override void CurrStatChanged()
    {
        if(currStatVal <= minStatVal)
        {
            OnDied();
        }
    }

    /// <summary>
    /// Invokes the Died event.
    /// </summary>
    protected virtual void OnDied()
    {
        isAlive = false;
        Debug.Log(gameObject.name + " has died");
        Died?.Invoke(this);
    }
}
