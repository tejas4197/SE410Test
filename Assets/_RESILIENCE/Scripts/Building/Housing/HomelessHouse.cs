using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// House that gets assigned to refugees when they are homeless. This is NOT an actual Building, but it behaves like one.
/// </summary>
public class HomelessHouse : House
{
    /// <summary>
    /// Sets the cached transform.
    /// </summary>
    private void OnEnable()
    {
        cachedTransform = transform;
    }

    /// <summary>
    /// Adds an occupant, but does not specify the "available space" that this "house" posseses to the manager.
    /// </summary>
    /// <param name="_r">Refugee to add.</param>
    public override void AddOccupant(Refugee _r)
    {

        occupants.Add(_r);
        _r.SetHouse(this);
    }

    /// <summary>
    /// Removes an occupant, but does not specify the "available space" that this "house" posseses to the manager.
    /// </summary>
    /// <returns>Refugee removed</returns>
    public override Refugee RemoveOccupant()
    {
        if (GetOccupantCount() == 0)
        {
            Debug.LogError("No Occupants to remove");
            return null;
        }

        Refugee _r = occupants[0];
        occupants.Remove(_r);
        _r.SetHouse(null);
        return _r;
    }
    /// <summary>
    /// Removes an occupant, but does not specify the "available space" that this "house" posseses to the manager.
    /// </summary>
    /// <param name="_r">Occupant to remove.</param>
    public override void RemoveOccupant(Refugee _r)
    {
        if (occupants.Contains(_r))
        {
            occupants.Remove(_r);
        }
    }
}
