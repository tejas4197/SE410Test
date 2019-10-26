using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Refugee class, handles all basic refugee assignment and stat modification.
/// </summary>
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Hydration))]
[RequireComponent(typeof(Food))]
[RequireComponent(typeof(Hygiene))]
[RequireComponent(typeof(OverallWellbeing))]
public class Refugee : MonoBehaviour
{
    #region Attributes
    [HideInInspector] public Transform cachedTransform;

    private House house;
    private Health health;
    private Hydration hydration;
    private Food food;
    private Hygiene hygiene;
    private OverallWellbeing wellbeing;

    [Header("Material settings")]
    [SerializeField] private Color healthyColor;
    [SerializeField] private Color unhealthyColor;
    private Material myMat;

    [Header("Stat UpdateSettings")]
    [Tooltip("Number of Ticks to wait between updating stat values.")]
    [SerializeField] private float updateStatTimer = 2f;
    [Tooltip("Amount to decrease health by, if decrease is required.")]
    [SerializeField] private int healthDecrease = 1;
    [Tooltip("Amount to increase health by, if decrease is required.")]
    [SerializeField] private int healthIncrease = 1;
    [Tooltip("Amount to decrease hydration by, if decrease is required.")]
    [SerializeField] private int hydrationDecrease = 1;
    [Tooltip("Amount to increase hydration by, if decrease is required.")]
    [SerializeField] private int hydrationIncrease = 1;
    [Tooltip("Amount to decrease food by, if decrease is required.")]
    [SerializeField] private int foodDecrease = 1;
    [Tooltip("Amount to increase food by, if decrease is required.")]
    [SerializeField] private int foodIncrease = 1;
    [Tooltip("Amount to decrease hygiene by, if decrease is required.")]
    [SerializeField] private int hygieneDecrease = 1;
    [Tooltip("Amount to increase hygiene by, if decrease is required.")]
    [SerializeField] private int hygieneIncrease = 1;


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
    /// Gets the food stat reference.
    /// </summary>
    /// <returns>Food stat reference for this refugee.</returns>
    public Food GetFood()
    {
        return food;
    }

    /// <summary>
    /// Gets the hygiene stat reference.
    /// </summary>
    /// <returns>Gets the hygiene stat reference.</returns>
    public Hygiene GetHygiene()
    {
        return hygiene;
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

    /// <summary>
    /// Sets up initial references to transform, health, hydration, food, hygiene and well being. As well as subscribes to events.
    /// </summary>
    private void Start()
    {
        cachedTransform = transform;
        health = GetComponent<Health>();
        hydration = GetComponent<Hydration>();
        food = GetComponent<Food>();
        hygiene = GetComponent<Hygiene>();
        wellbeing = GetComponent<OverallWellbeing>();

        myMat = GetComponent<Renderer>().material;

        health.Died += OnDiedEventHandler;
        TimeManager.GetInstance().OnTick += TickHandler;
    }
    #endregion

    #region PUBLIC_METHODS

    /// <summary>
    /// Sets a destination for the refugee to walk to.
    /// </summary>
    /// <param name="targetPos">Point to go to.</param>
    public void SetNewDestination(Vector3 targetPos)
    {
        this.GetComponent<NavMeshAgent>().SetDestination(targetPos);
    }

    /// <summary>
    /// Sets the refugees house that they live in. If they are in the HomelessHouse, it lets the housing manager know that they are homeless.
    /// </summary>
    /// <param name="_h">House to live in</param>
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

    #endregion

    #region PRIVATE_METHODS

    /// <summary>
    /// Event handler for every Tick. Updates refugee stats.
    /// </summary>
    private void TickHandler()
    {
        currTime += Time.deltaTime;
        if(currTime >= updateStatTimer)
        {
            UpdateHealth();
            UpdateHydration();
            UpdateFood();
            UpdateHygiene();
            UpdateWellBeing();

            currTime = 0;
        }
    }

    /// <summary>
    /// Updates the refugee health stat
    /// </summary>
    private void UpdateHealth()
    {
        if(house == null || house.GetType() == typeof(HomelessHouse))
        {
            health.SubtractCurrStat(healthDecrease);
        }
        else
        {
            health.SetMaxStat(house.GetHealth().GetCurrStatVal());
            if (health.GetMaxStatVal() < health.GetCurrStatVal())
            {
                health.SubtractCurrStat(healthDecrease);
            }
            else
            {
                health.AddCurrStat(healthIncrease);
            }
        }
    }

    /// <summary>
    /// Updates the refugee hydration stat
    /// </summary>
    private void UpdateHydration()
    {
        if (house == null) //We don't want to subtract health from registered homeless currently.
        {
            hydration.SubtractCurrStat(hydrationDecrease);
        }
        else if (!(house.GetType() == typeof(HomelessHouse)))
        {
            hydration.SetMaxStat(house.GetWaterHealth());

            if(hydration.GetMaxStatVal() < hydration.GetCurrStatVal())
            {
                hydration.SubtractCurrStat(hydrationDecrease);
            }
            else
            {
                hydration.AddCurrStat(hydrationIncrease);
            }
        }
    }

    /// <summary>
    /// Updates the refugee food stat.
    /// </summary>
    private void UpdateFood()
    {
        if(house == null)
        {
            food.SubtractCurrStat(foodDecrease);
        }
        else if(!(house.GetType() == typeof(HomelessHouse)))
        {
            food.SetMaxStat(house.GetFoodHealth());
            if (food.GetMaxStatVal() < food.GetCurrStatVal())
            {
                food.SubtractCurrStat(foodDecrease);
            }
            else
            {
                food.AddCurrStat(foodIncrease);
            }
        }
    }

    /// <summary>
    /// Updates the refugee hygiene stat.
    /// </summary>
    private void UpdateHygiene()
    {
        if(house == null)
        {
            hygiene.SubtractCurrStat(hydrationDecrease);
        }
        else if(!(house.GetType() == typeof(HomelessHouse)))
        {
            hygiene.AddCurrStat(hygieneIncrease);
        }
    }

    /// <summary>
    /// Updates the refugee well being stat.
    /// </summary>
    private void UpdateWellBeing()
    {
        wellbeing.SetCurrStat(health.GetCurrStatVal(), hydration.GetCurrStatVal());

        myMat.SetColor("_BaseColor", Color.Lerp(unhealthyColor, healthyColor, wellbeing.GetCurrStatVal()*1f / wellbeing.GetMaxStatVal()));
    }

    /// <summary>
    /// Event handler called when the health stat goes to 0. Removes refugee from thier house and sets the refugee inactive.
    /// </summary>
    /// <param name="source"></param>
    private void OnDiedEventHandler(object source)
    {
        if (house != null)
        {
            house.RemoveOccupant(this);
            house = null;
        }

        this.gameObject.SetActive(false);
    }

    #endregion

}

