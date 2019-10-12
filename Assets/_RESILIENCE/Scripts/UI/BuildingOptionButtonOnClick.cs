using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that gets called from onClick events on building buttons. The buttons pass in a prefab for this function to instanciate.
/// </summary>
public class BuildingOptionButtonOnClick : MonoBehaviour
{

    /// <summary>
    /// Triggers the action in BuildingMenuManager that starts building placement
    /// </summary>
    public void SendBuildingPlaceEvent(GameObject buildingPrefab)
    {
       BuildingMenuManager.GetInstance().InvokeOnBuildingSelectedToPlace(buildingPrefab);
    }
}
