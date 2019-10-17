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
    private Transform cachedTransform;
    private RefugeeManager refugeeManager;
    private HousingManager housingManager;

    #endregion

    /// <summary>
    /// Assigns references and begins the restore hydration process.
    /// </summary>
    void Start()
    {
        cachedTransform = transform;
        refugeeManager = RefugeeManager.GetInstance();
        housingManager = HousingManager.GetInstance();
        //StartCoroutine(RestoreHydrationUpdate(RefugeeManager.GetInstance().GetUpdateHealthTimer()));
    }
    /// <summary>
    /// Restores Hydration to refugees every "timeToWait" seconds.
    /// </summary>
    /// <param name="timeToWait">time to wait between restoring hydration 1 tick.</param>
    /// <returns>IEnumerator, runs in background.</returns>
    IEnumerator RestoreHydrationUpdate(int timeToWait)
    {
        while (true)
        {
            if (shouldRestoreHydration)
            {
                FireRestoreHydration();
            }
            yield return new WaitForTicks(timeToWait);
        }
    }

    /// <summary>
    /// Calls the code to add hydration to all refugees within a set range.
    /// </summary>
    private void FireRestoreHydration()
    {
        float distance;

        for (int i = 0; i < housingManager.GetHousesCount(); ++i)
        {
            distance = Vector3.Distance(refugeeManager.GetRefugeeAtIndex(i).cachedTransform.position, cachedTransform.position);
            if (distance < hydrationRange)
            {
                //refugeeManager.GetRefugeeAtIndex(i).SetHasHydration(true);
                //refugeeManager.GetRefugeeAtIndex(i).GetHydration().AddCurrStat(hydrationToRestore);
            }
        }
    }
}
