using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Singleton; 
/// Instantiates the buttons to build buildings; 
/// When a building button is clicked, sends the event to the BuildingPlacementUpdate script
/// </summary>
public class BuildingMenuManager : Singleton<BuildingMenuManager>
{
    // TODO: A Database of buildings that then auto generates UI buttons
    // Google Sheets or Airtable or something to store all the information
    // Parse Data
    // Pull it into Buttons
    // Construction

    // TODO: refactor this script's ui handling when more build UI buttons are added
    // Right now to toggle the UI, this script does a very simple onButtonPress
    // activate/deactivate of the two below gameobjects.

    // In a proper implementation of this script, there would be a 
    // Model/View/Controller implementation that has a Model that is storing the
    // building options and a Controller that loads the building options into
    // a single buildingOptionsContainer when a category button is clicked.

    // Or something else similarly scalable.

    #region VARIABLES

    /// <summary> UI Panel where housing building options are </summary>
    [SerializeField, Tooltip("UI Panel where housing building options are")]
    GameObject shelterOptionsContainer;

    /// <summary> UI Panel where water building options are </summary>
    [SerializeField, Tooltip("UI Panel where water building options are")]
    GameObject waterOptionsContainer;

    /// <summary> building prefab for building selected to be built </summary>
    GameObject selectedBuilding;

    #endregion VARIABLES

    #region GETTERS_SETTERS

    /// <summary>
    /// Getter for selectedBuilding (prefab for building placement indicator)
    /// </summary>
    /// <returns> current selectedBuilding</returns>
    public GameObject GetSelectedBuilding()
    {
        return selectedBuilding;
    }

    /// <summary>
    /// Setter for selectedBuilding (prefab for building placement indicator)
    /// </summary>
    /// <param name="_selectedBuilding">New selectedBuilding</param>
    public void SetSelectedBuilding(GameObject _selectedBuilding)
    {
        selectedBuilding = _selectedBuilding;
    }

    #endregion GETTERS_SETTERS

    #region EVENTS

    /// <summary>
    /// Delegate for subscribing to the OnBuildingSelected event
    /// </summary>
    public delegate void BuildingSelectedToPlaceHandler();

    /// <summary>
    /// Event invoked when a building has been selected for placement
    /// </summary>
    public event BuildingSelectedToPlaceHandler OnBuildingSelectedToPlace;
    #endregion EVENTS

    #region PUBLIC_METHODS

    /// <summary>
    /// Called by OnClick of Shelter Button; 
    /// Toggles shelter building options on (needs refactor)
    /// </summary>
    public void ShelterCategoryButtonPressed()
    {
        waterOptionsContainer.SetActive(false);
        shelterOptionsContainer.SetActive(true);
    }

    /// <summary>
    /// Called by OnClick of Water Button; 
    /// Toggles water building options on (needs refactor)
    /// </summary>
    public void WaterCategoryButtonPressed()
    {
        //toggle water building options on
        shelterOptionsContainer.SetActive(false);
        waterOptionsContainer.SetActive(true);
    }

    /// <summary>
    /// Called by BuiltingOptionButton script; 
    /// Triggers event that starts BuildingPlacementUpdate script coroutine
    /// </summary>
    /// <param name="_buildingToPlace">Building preview prefab for placement</param>
    public void InvokeOnBuildingSelectedToPlace(GameObject _buildingToPlace)
    {
        selectedBuilding = _buildingToPlace;
        OnBuildingSelectedToPlace?.Invoke();
    }

    #endregion PUBLIC_METHODS

}
