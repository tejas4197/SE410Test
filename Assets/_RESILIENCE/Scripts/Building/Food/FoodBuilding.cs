using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBuilding : Building
{
    [SerializeField] private int prefRadius;
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
}
