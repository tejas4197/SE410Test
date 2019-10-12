using System.Collections;
using System.Collections.Generic;

using UnityEngine;

// TODO: Import BuildingIndicatorUpdate script into this script

/// <summary>
/// Listens to the BuildingMenuManager for an event when a building button is clicked
/// Starts and ends the coroutine to place a building
/// </summary>
public class BuildingPlacementUpdate : MonoBehaviour
{
    #region VARIABLES

    // TODO: camera reference should probably be made static and put in Player script singleton, when that script is refactored
    /// <summary> Instance of Camera attached to player </summary>
    [SerializeField, Tooltip("Instance of Camera attached to player")]
    Camera playerCamera;

    /// <summary> Instance of BuildingMenuManager singleton, which fires the event for when the user wants to place a building </summary>
    BuildingMenuManager buildMenuInstance;

    /// <summary> Coroutine for placing a building, to ensure there is only ever one running </summary>
    Coroutine placementCoroutine = null;

    /// <summary> Instance of the building spawned during coroutine to preview building placement </summary>
    GameObject buildingPlacementIndicatorInstance;

    /// <summary> Current placement position of building </summary>
    Vector3 position;

    /// <summary> Tracks if player is holding shift, which allows placement of multiple buildings </summary>
    bool isShiftPressed;

    /// <summary> Starting position for buildingPreviewInstance prefab (just somewhere out of the way) </summary>
    Vector3 startingPosition = new Vector3(0, -50, 0);

    /// <summary> Value representing the layer that the ground plane is on </summary>
    const int groundPlaneLayerMask = 1 << 9; // layer # 9 needs to be bit shifted from some reason

    #endregion VARIABLES

    #region MONOBEHAVIOUR

    private void Start()
    {
        buildMenuInstance = BuildingMenuManager.GetInstance();
        buildMenuInstance.OnBuildingSelectedToPlace += StartPlaceBuilding;
    }

    // TODO: eventually all checking for input on update should be moved into a single script w events
    // I made this self contained just to make sure it would work during the merge
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (placementCoroutine != null)
            {
                BuildBuilding(); //TODO: add check to make sure you're not over UI
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            isShiftPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            isShiftPressed = false;
        }
        if (Input.GetKeyUp(KeyCode.Escape) && placementCoroutine != null)
        {
            CancelBuildingPlacement();
        }
    }

    private void OnDestroy()
    {
        buildMenuInstance.OnBuildingSelectedToPlace -= StartPlaceBuilding;
    }

    #endregion MONOBEHAVIOUR

    #region PRIVATE_METHODS

    /// <summary>
    /// Subscribed to the BuildingMenuManager event OnBuildingSelectedToPlace, which is fired by the player pressing a building button;
    /// Spawns in a preview of the building and starts the Place coroutine
    /// </summary>
    private void StartPlaceBuilding()
    {
        GameObject buildingPrefab = buildMenuInstance.GetSelectedBuilding();
        if (placementCoroutine != null) // there is already a placement coroutine going
        {
            CancelBuildingPlacement();
        }
        buildingPlacementIndicatorInstance = Instantiate(buildingPrefab, startingPosition, Quaternion.identity, BuildingManager.GetInstance().transform);
        placementCoroutine = StartCoroutine(UpdatePlacement());
    }

    /// <summary>
    /// Casts a ray from mouse to ground to update position of building preview
    /// </summary>
    /// <returns> Coroutine yield return null (runs every frame) </returns>
    private IEnumerator UpdatePlacement()
    {
        while (true)
        {
            Vector3 playerPosition = Input.mousePosition;
            Ray ray = playerCamera.ScreenPointToRay(playerPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundPlaneLayerMask))
            {
                position = RoundToWholeUnit(hit.point);
            }
            buildingPlacementIndicatorInstance.transform.position = position;
            yield return null;
        }
    }

    /// <summary>
    /// When player confirms building position, places building at selected position.
    /// Ends the coroutine unless shift is pressed
    /// </summary>
    private void BuildBuilding()
    {
        BuildingPlacementIndicatorUpdate placementIndicator = GetBuildIndicator();
        if (placementIndicator != null)
        {
            if (!placementIndicator.GetIsValid()) // not a valid placement position
            {
                return;
            }

        }
        if (!isShiftPressed)
        {
            EndPlaceBuildingCoroutine();
        }
    }

    /// <summary>
    /// Gets BuildingPlacementIndicatorUpdate script on building preview instance.
    /// Will log an error if BuildingPlacementIndicatorUpdate script is missing
    /// </summary>
    /// <returns>The BuildingPlacementIndicatorUpdate script if found, null if not found</returns>
    private BuildingPlacementIndicatorUpdate GetBuildIndicator()
    {
        if (buildingPlacementIndicatorInstance.GetComponent<BuildingPlacementIndicatorUpdate>())
        {
            return buildingPlacementIndicatorInstance.GetComponent<BuildingPlacementIndicatorUpdate>();
        }
        else
        {
            Debug.LogError("Building Placement Indicator is missing the Placement Indicator Script!");
            return null;
        }
    }

    /// <summary>
    /// Ends the coroutine and destroys the building instance
    /// </summary>
    private void CancelBuildingPlacement()
    {
        Destroy(buildingPlacementIndicatorInstance);
        buildingPlacementIndicatorInstance = null;
        StopCoroutine(placementCoroutine);
        placementCoroutine = null;
    }

    /// <summary>
    /// Ends the coroutine and resets the buildingPreviewInstance and placementCoroutine to null
    /// </summary>
    private void EndPlaceBuildingCoroutine()
    {
        buildingPlacementIndicatorInstance.GetComponent<Building>().BuildingPlaced();
        //Destroy(buildingPlacementIndicatorInstance.GetComponent<Rigidbody>());
        buildingPlacementIndicatorInstance.GetComponent<BuildingPlacementIndicatorUpdate>().ResetMaterials();
        buildingPlacementIndicatorInstance.GetComponent<BuildingPlacementIndicatorUpdate>().enabled = false;
        IncomeManager.GetInstance().SpendFunds(buildingPlacementIndicatorInstance.GetComponent<Building>().GetPrice());
        buildingPlacementIndicatorInstance = null;
        StopCoroutine(placementCoroutine);
        placementCoroutine = null;
    }

    /// <summary>
    /// Rounds a Vector3's x y and z to whole numbers to snap position to unity grid
    /// </summary>
    /// <param name="_vec">Vector3 to round</param>
    /// <returns>Rounded Vector3</returns>
    private Vector3 RoundToWholeUnit(Vector3 _vec)
    {
        Vector3 vec = new Vector3(RoundFloat(_vec.x), RoundFloat(_vec.y), RoundFloat(_vec.z));
        return vec;
    }

    /// <summary>
    /// Rounds a float to whole numbers
    /// </summary>
    /// <param name="_num">Float to round</param>
    /// <returns>Rounded float</returns>
    private float RoundFloat(float _num)
    {
        return Mathf.RoundToInt(_num);
    }

    #endregion PRIVATE_METHODS
}
