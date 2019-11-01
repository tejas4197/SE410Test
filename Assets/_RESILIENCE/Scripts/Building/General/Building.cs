using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: Rename this to Building

// Change how interactPopup object is handled (Need to change how popups and buildings work since each building is different)
// Building - Have 3 stats
//    - Durability (How quickly health goes down)
//    - Capacity (On houses occupancy, on food/water it's radius)
//      - A "preferred" level and a "crowded" level for capacity so likely two variables

/// <summary>
/// Building class represents the base structure from where we will derive houses, water buildings, etc. contains references to key things every building will require.
/// </summary>
[RequireComponent(typeof(Health))]
public abstract class Building : MonoBehaviour
{
    #region Attributes
    [SerializeField, Required]
    protected GameObject interactPopup;

    [SerializeField] protected float constructionTime = 5f;
    [SerializeField] protected int price;

    /// <summary>
    /// Cost to upgrade this building
    /// </summary>
    [MinValue(0), SerializeField]
    protected int upgradePrice;

    /// <summary>
    /// Level to which this building has been upgraded (starting at 0)
    /// </summary>
    [MinValue(0), ReadOnly, SerializeField]
    protected int upgradeLevel = 0;

    /// <summary>
    /// Quantity to which upgraded buildings' maximum and preferred capacity increase
    /// </summary>
    [MinValue(1), SerializeField]
    protected float upgradeMultiplier = 1.5f;

    protected Health health;
    private int hashVal;

    [HideInInspector] public Transform cachedTransform;
    #endregion

    #region Getters_Setters

    /// <summary>
    /// Returns the HashCode of this object
    /// </summary>
    /// <returns>int hash Code.</returns>
    public int GetHashVal()
    {
        return hashVal;
    }

    /// <summary>
    /// Returns the health class instance.
    /// </summary>
    /// <returns>Health class instance.</returns>
    public Health GetHealth()
    {
        return health;
    }

    /// <summary>
    /// Returns the price to construct the building.
    /// </summary>
    /// <returns>int, Price of Building.</returns>
    public int GetPrice() //Temp until we find a better place
    {
        return price;
    }

    /// <summary>
    /// Returns the price to upgrade the building
    /// </summary>
    /// <returns></returns>
    public int GetUpgradePrice()
    {
        return upgradePrice;
    }

    /// <summary>
    /// Returns the current level the building is upgraded to (starting at 0)
    /// </summary>
    public int GetUpgradeLevel()
    {
        return upgradeLevel;
    }

    /// <summary>
    /// Returns the stat multiplier that the building's stats are increased by upon upgrade
    /// </summary>
    public float GetUpgradeMultiplier()
    {
        return upgradeMultiplier;
    }

    #endregion

    /// <summary>
    /// Sets up stat references as well as turns interation popup off.
    /// </summary>
    protected void Start()
    {
        health = GetComponent<Health>();
        hashVal = GetHashCode();
        interactPopup.SetActive(false);
        cachedTransform = transform;
        customStart();
    }

    /// <summary>
    /// Completes Start tasks while not interfiering with the Monobehaviour Start
    /// </summary>
    protected abstract void customStart();

    /// <summary>
    /// Gets called when the building is placed on the ground at a location
    /// </summary>
    public abstract void BuildingPlaced();

    /// <summary>
    /// Coroutine used to "Construct" the building, if we would like a construction time, this gets called after being placed and is required to be called before it is "useable"
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator BuildingConstructionBegin();

    /// <summary>
    /// Handles building stat changes during an upgrade
    /// </summary>
    public abstract void Upgrade();

    /// <summary>
    /// Interact popup turns on.
    /// </summary>
    public void OnInteract()
    {
        interactPopup.gameObject.SetActive(true);
    }

    /// <summary>
    /// Interact popup turns off.
    /// </summary>
    public void OnDeselected()
    {
        interactPopup.gameObject.SetActive(false);
    }
}

