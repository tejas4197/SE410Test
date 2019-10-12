using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to attach to the building placement indicator to show valid placement; 
/// Stores the appropriate building prefab (for once it is built); 
/// Tracks whether building placement is valid and passes data to BuildingPlacementUpdate script
/// </summary>
public class BuildingPlacementIndicatorUpdate : MonoBehaviour
{

    #region VARIABLES

    /// <summary> Tracks if building is currently in empty lot </summary>
    bool isValid;

    /// <summary> Number of other buildings that the building is currently overlapping </summary>
    [SerializeField] int numColliding;

    /// <summary> Array of material renders on object (for updating texture with valid or invalid material) </summary>
    Renderer[] renderers;
    List<Material> origMaterials = new List<Material>();

    /// <summary> Material for showing the building is currently in valid place </summary>
    [SerializeField, Tooltip("Material for showing the building is currently in valid place")]
    Material holoMtrl;

    /// <summary> Material for showing the building is currently in invalid place </summary>
    [SerializeField, Tooltip("Material for showing the building is currently in invalid place")]
    private Color invalid;

    [SerializeField] private Color valid;

    private Building referencedBuilding;

    #endregion VARIABLES

    #region GETTERS_SETTERS

    /// <summary>
    /// Getter for isValid (Tracks if building is currently in empty lot)
    /// </summary>
    /// <returns>Current isValid</returns>
    public bool GetIsValid()
    {
        return isValid;
    }

    #endregion GETTERS_SETTERS

    #region MONOBEHAVIOUR

    private void Start()
    {
        referencedBuilding = GetComponent<Building>();
        renderers = GetComponentsInChildren<Renderer>();

        for(int i = 0; i < renderers.Length; ++i)
        {
            Material[] mats = renderers[i].materials;
            origMaterials.AddRange(mats);
            mats[0] = holoMtrl;
            renderers[i].materials = mats;
        }

        numColliding = 0;
        SetMaterialValid();
        IncomeManager.GetInstance().OnMoneyChanged += MoneyChangedHandler;
    }

    private void OnTriggerEnter(Collider other)
    {
        numColliding++;
        SetMaterialInvalid();
    }

    private void OnTriggerExit(Collider other)
    {
        numColliding--;
        if (numColliding == 0)
        {
            SetMaterialValid();
        }
    }

    private void OnDisable()
    {
        IncomeManager.GetInstance().OnMoneyChanged -= MoneyChangedHandler;
    }

    private void OnDestroy()
    {
        IncomeManager.GetInstance().OnMoneyChanged -= MoneyChangedHandler;
    }

    #endregion MONOBEHAVIOUR

    #region PRIVATE_METHODS
    /// <summary>
    /// Sets all materials on the model to the invalid material
    /// </summary>
    private void SetMaterialInvalid()
    {
        isValid = false;
        holoMtrl.SetColor("_BaseColor", invalid);
    }

    /// <summary>
    /// Sets all materials on the model to the valid material
    /// </summary>
    private void SetMaterialValid()
    {
        if (IncomeManager.GetInstance().CanSpendFunds(referencedBuilding.GetPrice()))
        {
            isValid = true;
            holoMtrl.SetColor("_BaseColor", valid);
        }
        else
        {
            isValid = false;
            Debug.LogWarning("Cannot afford building");
            SetMaterialInvalid();
        }
    }

    public void ResetMaterials()
    {
        int count = 0;
        for (int i = 0; i < renderers.Length; ++i)
        {
            Material[] mats = renderers[i].materials;
            for (int j = 0; j < renderers[i].materials.Length; ++j)
            {
                mats[j] = origMaterials[count++];
            }
            renderers[i].materials = mats;
        }
    }

    /// <summary>
    /// Event handler for when the player's current funds change.
    /// </summary>
    /// <param name="_money">The current amount of funds.</param>
    private void MoneyChangedHandler(int _money)
    {
        if (numColliding > 0)
            return;
        SetMaterialValid();
    }
    #endregion PRIVATE_METHODS

}
