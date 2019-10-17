using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class IncomeManager : Singleton<IncomeManager>
{
    #region variables
    [ReadOnly, MinValue(0)]
    [SerializeField, Tooltip("current funds player has in game")]
    private int funds;

    [MinValue(0)]
    [SerializeField, Tooltip("starting funds at beginning of game")]
    public int startingFunds = 100;

    [Header("monthly income calculations")]
    [MinValue(0f), MaxValue(1f)]
    [SerializeField, Tooltip("weight of camp success in monthly income")]
    public float weight1 = 0.5f;

    [MinValue(0f), MaxValue(1f)]
    [SerializeField, Tooltip("weight of camp need in monthly income")]
    public float weight2 = 0.5f;

    [MinValue(0)]
    [SerializeField, Tooltip("multiplier for monthly income, based off cheapest building cost")]
    public int multiplier = 50;

    [ShowInInspector, ReadOnly]
    [Tooltip("Number of Refugees in Camp")]
    private int refugeeVolume;

    [ShowInInspector, ReadOnly]
    [Tooltip("Avg Health of Refugees in Camp")]
    private float refugeeHealth;
    private int refugeesResettled;
    private RefugeeManager refugeeManager;

    #endregion variables

    #region Events
    public delegate void MoneyChangedHandler(int _money);
    public event MoneyChangedHandler OnMoneyChanged;
    #endregion

    #region GETTERS_SETTERS

    /// <summary>
    /// Accessor for current funds
    /// </summary>
    public int GetFunds()
    {
        return funds;
    }

    /// <summary>
    /// Mutator for current funds
    /// </summary>
    /// <param name="_funds">New value of funds</param>
    private void SetFunds(int _funds)
    {
        funds = _funds;
    }

    #endregion GETTERS_SETTERS

    /// <summary>
    /// Adds income based on success and need
    /// </summary>
    public void AddMonthlyIncome()
    {
        UpdateRefugeeValues();

        // for readability and in case we ever want to display these
        int successIncome = Mathf.RoundToInt((weight1 * Mathf.Log(refugeeHealth * refugeeVolume + refugeesResettled)) * multiplier);
        int needIncome = Mathf.RoundToInt((weight2 * Mathf.Log(refugeeVolume) + 1) * multiplier);
        int monthlyIncome = successIncome + needIncome;

        funds += monthlyIncome;
        Debug.Log("Monthly Income: " + successIncome + "(s) + " + needIncome + "(n) = " + monthlyIncome);
        UIManager.GetInstance().SetFundsText(funds);
        OnMoneyChanged?.Invoke(funds);
    }

    /// <summary>
    /// Adds a specified amount of income from an event
    /// </summary>
    /// <param name="_eventFunds">Income to add</param>
    public void AddEventIncome(int _eventFunds)
    {
        funds += _eventFunds;
        UIManager.GetInstance().SetFundsText(funds);
        OnMoneyChanged?.Invoke(funds);
    }

    /// <summary>
    /// Reduces income by a specified amount
    /// </summary>
    /// <param name="_spentFunds">Income to spend</param>
    public bool SpendFunds(int _spentFunds)
    {
        if (_spentFunds <= funds)
        {
            funds -= _spentFunds;
            UIManager.GetInstance().SetFundsText(funds);
            OnMoneyChanged?.Invoke(funds);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if income can be reduced by specified amount
    /// </summary>
    /// <param name="_spentFunds">Income to spend</param>
    public bool CanSpendFunds(int _spentFunds)
    {
        if (_spentFunds <= funds)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Updates stats of refugee camp for accurate funding
    /// </summary>
    private void UpdateRefugeeValues()
    {
        refugeeVolume = refugeeManager.GetRefugeeCount();

        float avg = 0f;
        foreach (Refugee r in refugeeManager.GetRefugees())
        {
            avg += r.GetOverallWellbeing().GetCurrStatVal();
        }
        refugeeHealth = avg / refugeeManager.GetRefugees().Count;

        // To be implemented w/ resettlement mechanic
        refugeesResettled = 0;
    }


    #region MONOBEHAVIOR
    private void Start()
    {
        refugeeManager = RefugeeManager.GetInstance();
        SetFunds(startingFunds);
        UIManager.GetInstance().SetFundsText(funds);
        TimeManager.GetInstance().OnMonthPass += AddMonthlyIncome;
    }

    private void OnDisable()
    {
        TimeManager.GetInstance().OnMonthPass -= AddMonthlyIncome;
    }
    #endregion MONOBEHAVIOR
}
