using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Water Pump Building. has a radius of effect and a radius that it effects to a lesser degree.
/// </summary>
public class Water : Building
{
    [SerializeField] private int prefRadius;
    [SerializeField] private int maxRadius;

    /// <summary>
    /// Currently does nothing, but is required to be overrode.
    /// </summary>
    protected override void customStart()
    {
    }

    /// <summary>
    /// Waits for the construction to finish.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator BuildingConstructionBegin()
    {
        yield return new WaitForSeconds(constructionTime);
    }

    /// <summary>
    /// Tells the Building construction process to start.
    /// </summary>
    public override void BuildingPlaced()
    {
        StartCoroutine(BuildingConstructionBegin());
    }
}
