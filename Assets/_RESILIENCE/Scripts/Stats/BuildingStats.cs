using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New House/House")]
public class BuildingStats : ScriptableObject
{
    //possibly add building levels which then autopopulates the other stats based on levels

    #region VARIABLES
    //TODO: Add range for bldg capacity based on design guidelines
    [Tooltip("Max capacity of the building")]
    public int buildingOccupancy = 0;

    [Tooltip("the optimal capacity to occupy a house")]
    public int preferredOccupancy = 0;

    [Tooltip("The rate at which the building's health would reduce over time")]
    [Range(0f, 1f)]
    public float deteriorationRate;

    //[Tooltip("Check true if the refugees can occupy the building and false if they cannot occupy")]
   // public bool canOccupy;
    #endregion

    #region GETTERS_SETTERS
    public int  getBuildingOccupancy()
    {
        return buildingOccupancy;
    }
    public int getPreferredOccupancy()
    {
        return preferredOccupancy;
    }
    public float getDeteriorationRate()
    {
        return deteriorationRate;
    }
    #endregion
}
