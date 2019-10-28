using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// House is a building that refugees will live in. It has a prefered and max occupancy for living. Health, durability, price, etc.
/// </summary>
public class House : Building
{
    #region VARIABLES
    [SerializeField] protected int prefOccupancy;
    [SerializeField] protected int maxOccupancy;


    protected List<Refugee> occupants = new List<Refugee>();

    private List<Health> waterHealth = new List<Health>();

    #endregion VARIABLES

    #region GETTERS_SETTERS

    public int GetPrefOccupancy()
    {
        return prefOccupancy;
    }

    public int GetMaxOccupancy()
    {
        return maxOccupancy;
    }

    public int GetOccupantCount()
    {
        return occupants.Count;
    }

    public int GetWaterHealth()
    {
        if (waterHealth.Count == 0)
        {
            return 0;
        }
        int highestCurrentHealth = 0;
        foreach (Health h in waterHealth)
        {
            if (h.GetCurrStatVal() > highestCurrentHealth)
            {
                highestCurrentHealth = h.GetCurrStatVal();
            }
        }
        return highestCurrentHealth;
    }

    #endregion GETTERS_SETTERS
    /// <summary>
    /// Currently does nothing, but is required as per the abstract class.
    /// </summary>
    protected override void customStart()
    {
    }

    /// <summary>
    /// Initiates the interactable Popup and starts the construction process.
    /// </summary>
    public override void BuildingPlaced()
    {
        interactPopup.GetComponent<HousePopup>().SetHouse(this);
        StartCoroutine(BuildingConstructionBegin());
    }

    /// <summary>
    /// Waits for construction to finish and tells the HousingManager that it is ready.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator BuildingConstructionBegin()
    {
        yield return new WaitForTicks(constructionTime);

        HousingManager.GetInstance().NewHouseBuilt(this);
    }

    public override void Upgrade()
    {
        prefOccupancy = Mathf.RoundToInt(prefOccupancy * upgradeMultiplier);
        maxOccupancy = Mathf.RoundToInt(maxOccupancy * upgradeMultiplier);
        health.RestoreHealth();
        // increase max health and/or reduce deterioration rate
        upgradeLevel++;
    }

    #region PUBLIC_METHODS

    /// <summary>
    /// Adds occupant _r into this house.
    /// </summary>
    /// <param name="_r">Refugee to add to this house.</param>
    public virtual void AddOccupant(Refugee _r)
    {
        if (maxOccupancy == GetOccupantCount())
        {
            Debug.LogError(gameObject.name + " is already a full house.");
            return;
        }

        occupants.Add(_r);
        _r.SetHouse(this);
        if (GetOccupantCount() < prefOccupancy)
        {
            HousingManager.GetInstance().AddBelowPrefOccupants(GetHashVal(), this);
        }
        else if (GetOccupantCount() < maxOccupancy)
        {
            HousingManager.GetInstance().RemoveBelowPrefOccupants(GetHashVal());
            HousingManager.GetInstance().AddBelowMaxOccupants(GetHashVal(), this);
        }
        else
        {
            HousingManager.GetInstance().RemoveBelowMaxOccupants(GetHashVal());
        }
    }

    /// <summary>
    /// Removes a refugee from this house.
    /// </summary>
    /// <returns>Refugee removed.</returns>
    public virtual Refugee RemoveOccupant()
    {
        if (GetOccupantCount() == 0)
        {
            Debug.LogError("No Occupants to remove");
            return null;
        }

        Refugee _r = occupants[0];
        occupants.Remove(_r);
        _r.SetHouse(null);

        if (GetOccupantCount() < prefOccupancy)
        {
            HousingManager.GetInstance().AddBelowPrefOccupants(GetHashVal(), this);
            HousingManager.GetInstance().RemoveBelowMaxOccupants(GetHashVal());
        }
        else if (GetOccupantCount() < maxOccupancy)
        {
            HousingManager.GetInstance().AddBelowMaxOccupants(GetHashVal(), this);
        }
        return _r;
    }

    /// <summary>
    /// Removes specified occupant
    /// </summary>
    /// <param name="_r">Occupant to remove.</param>
    public virtual void RemoveOccupant(Refugee _r)
    {
        if (occupants.Contains(_r))
        {
            occupants.Remove(_r);
        }
    }

    public void AddWaterSource(Health _waterHealth)
    {
        waterHealth.Add(_waterHealth);
    }

    #endregion PUBLIC_METHODS

}
