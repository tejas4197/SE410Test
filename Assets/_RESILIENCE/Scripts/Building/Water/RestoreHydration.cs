using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Restores the hydration stat over time to all refugees within a given distance to this.
/// </summary>
public class RestoreHydration : MonoBehaviour
{
    #region Attributes

    [SerializeField] private float hydrationRange = 5f;
    [SerializeField] private int hydrationToRestore = 1;
    [SerializeField] private bool shouldRestoreHydration;

    private bool onlyRestoreHomeless = false;

    private Transform cachedTransform;
    private HomelessHouse homelessHouse;

    #endregion

    #region MONOBEHAVIOUR
    /// <summary>
    /// Assigns references and begins the restore hydration process.
    /// </summary>
    void Start()
    {
        cachedTransform = transform;


        if(this.GetComponent<Building>().GetType() == typeof(ReceptionCenter))
        {
            onlyRestoreHomeless = true;
            homelessHouse = HousingManager.GetInstance().GetHomelessHouse();
        }

        StartCoroutine(RestoreHydrationUpdate(RefugeeManager.GetInstance().GetUpdateHealthTimer()));
    }

    #endregion

    #region PRIVATE_METHODS
    /// <summary>
    /// Restores Hydration to refugees every "timeToWait" seconds.
    /// </summary>
    /// <param name="timeToWait">time to wait between restoring hydration 1 tick.</param>
    /// <returns>IEnumerator, runs in background.</returns>
    private IEnumerator RestoreHydrationUpdate(float timeToWait)
    {
        while (true)
        { 
            if(PauseManager.GetInstance().GetIsPaused())
            {
                yield return null;
            }

            if (shouldRestoreHydration)
            {
                FireRestoreHydration();
            }
            yield return new WaitForSeconds(timeToWait);
        }
    }

    /// <summary>
    /// Calls the code to add hydration to all refugees within a set range.
    /// </summary>
    private void FireRestoreHydration()
    {
        float distance;

        if(onlyRestoreHomeless)
        {
            for(int i = 0; i < homelessHouse.GetOccupantCount(); ++i)
            {
                homelessHouse.GetOccupant(i).GetHydration().AddCurrStat(hydrationToRestore / 2); //TODO: Make this magic number a variable somewhere, GM?\
                Debug.Log(homelessHouse.GetOccupant(i).name);
            }
            return;
        }

        for(int i = 0; i < HousingManager.GetInstance().GetHouseCount(); ++i)
        {
            House curr = HousingManager.GetInstance().GetHouseAtIndex(i);
            distance = Vector3.Distance(curr.cachedTransform.position, cachedTransform.position);
            if(distance < hydrationRange)
            {
                for(int j = 0; j < curr.GetOccupantCount(); ++j)
                {
                    curr.GetOccupant(j).GetHydration().AddCurrStat(hydrationToRestore);
                }
            }
        }
    }

    #endregion

}
