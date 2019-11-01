using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Building for giving out food to refugees.
/// </summary>
public class FoodBuilding : Building
{
    [Min(0)]
    [SerializeField] private int prefRadius;
    [Min(0)]
    [SerializeField] private int maxRadius;


    /// <summary>
    /// Currently does nothing, but is required to be overrode.
    /// </summary>
    protected override void customStart()
    {
        Debug.Log("Custom Start!");
    }

    /// <summary>
    /// Waits for the construction to finish.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator BuildingConstructionBegin()
    {
        yield return new WaitForTicks(constructionTime);

        HousingManager housingManager = HousingManager.GetInstance();
        float distance;

        for (int i = 0; i < housingManager.GetHousesCount(); ++i)
        {
            distance = Vector3.Distance(housingManager.GetHouseAtIndex(i).cachedTransform.position, cachedTransform.position);
            if (distance < maxRadius)
            {
                housingManager.GetHouseAtIndex(i).AddFoodSource(health);
            }
        }
    }

    /// <summary>
    /// Tells the Building construction process to start.
    /// </summary>
    public override void BuildingPlaced()
    {
        StartCoroutine(BuildingConstructionBegin());
    }

    /// <summary>
    /// Upgrades the food Building.
    /// </summary>
    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }
}
