using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of currently homeless refugees
/// Allocates housing whenever a new house is built
/// </summary>
public class HousingManager : Singleton<HousingManager>
{
    #region VARIABLES
    /// <summary> List of available houses </summary>
    private List<House> houses = new List<House>();

    private Dictionary<int, House> belowPrefOccupants = new Dictionary<int, House>();
    private Dictionary<int, House> belowMaxOccupants = new Dictionary<int, House>();

    [SerializeField] private HomelessHouse homelessHouse;


    /// <summary> All refugees not currently assigned to housing </summary>
    private Queue<Refugee> homeless = new Queue<Refugee>();
    #endregion VARIABLES

    #region Getters_Setters

    public List<House> GetHouses()
    {
        return houses;
    }

    /// <summary>
    /// Gets a house from the houses list, at a given index.
    /// </summary>
    /// <param name="_index">Index of house wanted.</param>
    /// <returns>House at the given index.</returns>
    public House GetHouseAtIndex(int _index)
    {
        return houses[_index];
    }

    /// <summary>
    /// Returns the number of objects in the housing list.
    /// </summary>
    /// <returns>Int, Number of houses in housing list.</returns>
    public int GetHouseCount()
    {
        return houses.Count;
    }

    public Dictionary<int, House> GetBelowPrefOccupantsHouses()
    {
        return belowPrefOccupants;
    }

    public Dictionary<int, House> GetBelowMaxOccupantsHouses()
    {
        return belowMaxOccupants;
    }

    public Queue<Refugee> GetHomeless()
    {
        return homeless;
    }

    public Refugee DequeueHomeless()
    {
        return homeless.Dequeue();
    }

    public void EnqueueHomeless(Refugee _r)
    {
        homeless.Enqueue(_r);
    }

    /// <summary>
    /// Returns the reference to the Homeless House
    /// </summary>
    /// <returns>Reference to the Homeless House</returns>
    public HomelessHouse GetHomelessHouse()
    {
        return homelessHouse;
    }

    #endregion


    #region PUBLIC_METHODS

    public void NewHouseBuilt(House house)
    {
        houses.Add(house);

        if (house.GetType() == typeof(HomelessHouse))
        {
            return;
        }
        AddBelowPrefOccupants(house.GetHashVal(), house);
        GameManager.GetInstance().AddHousing(house.GetMaxOccupancy()); // TODO: This needs to be replaced when refactoring UI to look better

        // If there are any homeless, fill this house as full as possible,
        // because even overcrowded housing is better than being homeless
        if (homeless.Count > 0)
        {
            while (homeless.Count > 0 && house.GetMaxOccupancy() > house.GetOccupantCount())
            {
                Refugee _r = homeless.Dequeue();
                homelessHouse.RemoveOccupant(_r);
                house.AddOccupant(_r);
            }
        }

        // if there's still any preferred space, take some people out of overcrowded houses
        while (house.GetPrefOccupancy() > house.GetOccupantCount())
        {
            bool allHousingAtPreferredLevels = true;

            foreach (House h in houses)
            {
                // Take them out one by one so crowding levels lower uniformly across houses
                if (h.GetPrefOccupancy() < h.GetOccupantCount())
                {

                    house.AddOccupant(h.RemoveOccupant());
                    // See if we need to keep looping (so no infinite while loop)
                    if (h.GetPrefOccupancy() < h.GetOccupantCount())
                    {
                        allHousingAtPreferredLevels = false;
                    }
                }
            }
            // Stop looping because even though this house isn't full, there are no other refugees in overcrowded houses
            if (allHousingAtPreferredLevels)
            {
                return;
            }
        }
    }

    public House GetBestHouse()
    {
        foreach(House curr in belowPrefOccupants.Values)
        {
            return curr;
        }

        foreach(House curr in belowMaxOccupants.Values)
        {
            return curr;
        }
   
        return homelessHouse;
    }



    public bool IsBelowPrefOccupants(int _hashCode)
    {
        return belowPrefOccupants.ContainsKey(_hashCode);
    }

    public void AddBelowPrefOccupants(int _hashCode, House _h)
    {
        if (!IsBelowPrefOccupants(_hashCode))
        {
            belowPrefOccupants.Add(_hashCode, _h);
        }
    }

    public void RemoveBelowPrefOccupants(int _hashCode)
    {
        if (IsBelowPrefOccupants(_hashCode))
        {
            belowPrefOccupants.Remove(_hashCode);
        }
    }

    public bool IsBelowMaxOccupants(int _hashCode)
    {
        return belowMaxOccupants.ContainsKey(_hashCode);
    }

    public void AddBelowMaxOccupants(int _hashCode, House _h)
    {
        if (!IsBelowMaxOccupants(_hashCode))
        {
            belowMaxOccupants.Add(_hashCode, _h);
        }
    }

    public void RemoveBelowMaxOccupants(int _hashCode)
    {
        if(IsBelowMaxOccupants(_hashCode))
        {
            belowMaxOccupants.Remove(_hashCode);
        }
    }

    #endregion PUBLIC_METHODS
}
