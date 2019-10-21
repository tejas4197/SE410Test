using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Hydration))]
[RequireComponent(typeof(OverallWellbeing))]
public class Refugee : MonoBehaviour
{
    #region Attributes
    public Color healthyColor;
    public Color unhealthyColor;
    [HideInInspector] public Transform cachedTransform;

    private House house;
    private Health health;
    private Hydration hydration;
    private OverallWellbeing wellbeing;

    private Material myMat;

    private float updateStatTimer = 2f;
    private float currTime = 0;

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

    /// <summary>
    /// Accessor for OverallWellbeing
    /// </summary>
    public OverallWellbeing GetOverallWellbeing()
    {
        return wellbeing;
    }
    #endregion

    #region MONOBEHAVIOUR

    private void Start()
    {
        cachedTransform = transform;
        health = GetComponent<Health>();
        hydration = GetComponent<Hydration>();
        wellbeing = GetComponent<OverallWellbeing>();

        myMat = GetComponent<Renderer>().material;

        health.Died += OnDiedEventHandler;
        TimeManager.GetInstance().OnTick += TickHandler;
    }
    #endregion

    private void TickHandler()
    {
        currTime += Time.deltaTime;
        if(currTime >= updateStatTimer)
        {
            UpdateHealth();
            UpdateHydration();
            UpdateWellBeing();

            currTime = 0;
        }
    }

    private void UpdateHealth()
    {
        if(house == null || house.GetType() == typeof(HomelessHouse))
        {
            health.SubtractCurrStat(1);
        }
        else
        {
            Debug.Log(house);
            health.SetMaxStat(house.GetHealth().GetCurrStatVal());
            health.AddCurrStat(1);
        }
    }

    private void UpdateHydration()
    {
        if (house == null) //We don't want to subtract health from registered homeless currently.
        {
            hydration.SubtractCurrStat(1);
        }
        else if (!(house.GetType() == typeof(HomelessHouse)))
        {
            hydration.SetMaxStat(house.GetWaterHealth());
            health.AddCurrStat(1);
        }
    }

    private void UpdateWellBeing()
    {
        wellbeing.SetCurrStat(health.GetCurrStatVal(), hydration.GetCurrStatVal());

        myMat.SetColor("_BaseColor", Color.Lerp(unhealthyColor, healthyColor, wellbeing.GetCurrStatVal()*1f / wellbeing.GetMaxStatVal()));
    }

    public void SetNewDestination(Vector3 targetPos)
    {
        this.GetComponent<NavMeshAgent>().SetDestination(targetPos);
    }

    public void SetHouse(Building _h)
    {
        if (_h == null)
        {
            SetNewDestination(GameManager.GetInstance().receptionCenter.cachedTransform.position);
            return;
        }
        if (_h.GetType() == typeof(HomelessHouse))
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

