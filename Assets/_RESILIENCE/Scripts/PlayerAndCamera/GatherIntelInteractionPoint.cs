using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherIntelInteractionPoint : MonoBehaviour
{
    #region Attributes
    [SerializeField] private SkyUIController skyUiController;
    private float currTime;
    private float degradeFrequency;
    private GameObject playerGO;
    private Player player;
    private Health myHealth;
    #endregion

    void Start()
    {
        currTime = 0;
        degradeFrequency = .5f;

        player = GameManager.GetInstance().player;
        playerGO = player.gameObject;
        myHealth = GetComponent<Health>();

        myHealth.Died += OnDied;

        StartCoroutine(myUpdate());
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject != playerGO)
        {
            return;
        }

        if(player.interactButtonDown)
        {
            //TODO update sky stats with current info.
            skyUiController.UpdateHud(myHealth.GetCurrStatVal());
            Debug.Log(myHealth.GetCurrStatVal());
        }
    }

    /// <summary>
    /// This function calls out to the health script attached to the current GO and subtracts health by 3.
    /// </summary>
    private void DegradeHealth()
    {
        myHealth.SubtractCurrStat(3);
    }

    /// <summary>
    /// Instead of using update, we have a coroutine that can be stopped. Currently, the only functionality it has is to decrese its own health over time.
    /// </summary>
    /// <returns></returns>
    IEnumerator myUpdate()
    {
        while(true)
        {
            currTime += Time.deltaTime;
            if(currTime > degradeFrequency)
            {
                currTime = 0;
                DegradeHealth();
            }
            yield return null;
        }
    }

    /// <summary>
    /// This script listens to the health script attached waiting for it to die.
    /// </summary>
    /// <param name="source"> The object that the event was raised from.</param>
    private void OnDied(object source)
    {
        StopAllCoroutines();
        myHealth.Died -= OnDied;
        Debug.Log("GOT DEATH");
        this.gameObject.SetActive(false);
    }

}
