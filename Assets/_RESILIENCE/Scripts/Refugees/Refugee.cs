using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Hydration))]
public class Refugee : MonoBehaviour
{
    #region Attributes
    public Color healthyColor;
    public Color unhealthyColor;
    [HideInInspector] public Transform cachedTransform;

    private House house;

    private Health health;
    private Hydration hydration;

    #endregion

    #region GettersSetters
    /// <summary>
    /// Accessor for health value
    /// </summary>
    public Health GetHealth()
    {
        return health;
    }

    /// <summary>
    /// Gets the hydration curr value.
    /// </summary>
    /// <returns></returns>
    public Hydration GetHydration()
    {
        return hydration;
    }
    #endregion

    private void Start()
    {
        health = GetComponent<Health>();
        hydration = GetComponent<Hydration>();

        health.Died += OnDiedEventHandler;

        StartCoroutine(UpdateHealth(RefugeeManager.GetInstance().GetUpdateHealthTimer()));
        StartCoroutine(UpdateHydration(RefugeeManager.GetInstance().GetUpdateHealthTimer()));
        cachedTransform = transform;
    }

    private IEnumerator UpdateHealth(float timer)
    {
        while (true)
        {
            if (house == null)
            {
                health.SubtractCurrStat(1);
            }
            else
            {
                int healthGoal = house.GetHealth().GetMaxStatVal();
                if (healthGoal > health.GetCurrStatVal())
                    health.AddCurrStat(1);
                if (healthGoal < health.GetCurrStatVal())
                    health.SubtractCurrStat(1);
            }

            Renderer matRenderer = this.GetComponent<Renderer>();
            Material mat = new Material(matRenderer.material);
            mat.SetColor("_BaseColor", Color.Lerp(unhealthyColor, healthyColor, (health.GetCurrStatVal() * 1.0f) / 100.0f));
            matRenderer.material = mat;
            yield return new WaitForSeconds(timer);
        }
    }

    private IEnumerator UpdateHydration(float waitTime)
    {
        while(true)
        {
            hydration.SubtractCurrStat(2);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void SetNewDestination(Vector3 targetPos)
    {
        this.GetComponent<NavMeshAgent>().SetDestination(targetPos);
    }

    public void SetHouse(Building _h)
    {
        if(_h == null)
        {
            SetNewDestination(GameManager.GetInstance().receptionCenter.cachedTransform.position);
            return;
        }
        if(_h.GetType() == typeof(HomelessHouse))
        {
            HousingManager.GetInstance().EnqueueHomeless(this);
        }
        else
        {
            house = (House)_h;
        }
        SetNewDestination(_h.cachedTransform.position);
    }

    private void OnDiedEventHandler(object source)
    {
        if (house != null)
        {
            house.RemoveOccupant(this);
            house = null;
        }

        this.gameObject.SetActive(false);
    }

}

