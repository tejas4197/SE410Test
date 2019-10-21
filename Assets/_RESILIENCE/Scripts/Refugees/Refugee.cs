using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Hydration))]
[RequireComponent(typeof(Food))]
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
    private Food food;
    private OverallWellbeing wellbeing;

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

    private void Start()
    {
        health = GetComponent<Health>();
        hydration = GetComponent<Hydration>();
        food = GetComponent<Food>();
        wellbeing = GetComponent<OverallWellbeing>();

        health.Died += OnDiedEventHandler;

        StartCoroutine(UpdateHealth(RefugeeManager.GetInstance().GetUpdateHealthTimer()));
        StartCoroutine(UpdateHydration(RefugeeManager.GetInstance().GetUpdateHealthTimer()));
        StartCoroutine(UpdateWellBeing(RefugeeManager.GetInstance().GetUpdateHealthTimer()));
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
            yield return new WaitForSeconds(timer);
        }
    }

    private IEnumerator UpdateHydration(float waitTime)
    {
        while (true)
        {
            if (house == null)
            {
                hydration.SubtractCurrStat(1);
            }
            else
            {
                int healthGoal = house.GetWaterHealth();
                if (healthGoal > hydration.GetCurrStatVal())
                    hydration.AddCurrStat(1);
                if (healthGoal < hydration.GetCurrStatVal())
                    hydration.SubtractCurrStat(1);
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator UpdateWellBeing(float waitTime)
    {
        while (true)
        {
            wellbeing.SetCurrStat(health.GetCurrStatVal(), hydration.GetCurrStatVal());

            Renderer matRenderer = this.GetComponent<Renderer>();
            Material mat = new Material(matRenderer.material);
            mat.SetColor("_BaseColor", Color.Lerp(unhealthyColor, healthyColor, (wellbeing.GetCurrStatVal() * 1.0f) / 100.0f));
            matRenderer.material = mat;
            yield return new WaitForSeconds(waitTime);
        }
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

