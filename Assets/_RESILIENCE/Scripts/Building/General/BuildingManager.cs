using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum BuildingType
{
    NULL,
    HOUSE,
    WATER
}

public enum BuildingSize
{
    NULL,
    SMALL,
    MEDIUM,
    LARGE
}


[System.Serializable]
public struct BuildingPrefabs
{
    public BuildingType type;
    public BuildingSize size;
    public GameObject prefab;
}

/// <summary>
/// Manager class for everything involving buildings, and manages the different kinds of buildings that exist.
/// </summary>
public class BuildingManager : Singleton<BuildingManager>
{
    [SerializeField] private BuildingPrefabs[] buildings;

    #region Getters_Setters
    /// <summary>
    /// Gets the building prefab associated with the building type and size requested.
    /// </summary>
    /// <param name="_t">Building type.</param>
    /// <param name="_s">Building size.</param>
    /// <returns>Prefab for the specified building.</returns>
    public GameObject GetBuildingPrefab(BuildingType _t, BuildingSize _s)
    {
        for(int i = 0; i < buildings.Length; i++)
        {
            if(buildings[i].type == _t && buildings[i].size == _s)
            {
                return buildings[i].prefab;
            }
        }

        return new GameObject();
    }

    #endregion
}
