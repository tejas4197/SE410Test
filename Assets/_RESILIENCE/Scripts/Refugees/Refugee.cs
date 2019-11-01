using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Refugee class, handles all basic refugee assignment and stat modification.
/// </summary>
[RequireComponent(typeof(Shelter))]
[RequireComponent(typeof(Hydration))]
[RequireComponent(typeof(Food))]
[RequireComponent(typeof(Hygiene))]
[RequireComponent(typeof(OverallWellbeing))]
public class Refugee : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// Cached transform for performance perposes, used over this.transform
    /// </summary>
    [HideInInspector] public Transform cachedTransform;

    /// <summary>
    /// House the refugee lives in
    /// </summary>
    private House house;

    /// <summary>
    /// Reference to the refugee's shelter stat.
    /// </summary>
    private Shelter shelter;
    /// <summary>
    /// Reference to the refugee's hydration stat.
    /// </summary>
    private Hydration hydration;
    /// <summary>
    /// Reference to the refugee's food stat.
    /// </summary>
    private Food food;
    /// <summary>
    /// Reference to the refugee's hygiene stat.
    /// </summary>
    private Hygiene hygiene;
    /// <summary>
    /// Reference to the refugee's wellbeing stat.
    /// </summary>
    private OverallWellbeing wellbeing;

    /// <summary>
    /// Color the refugee takes when healthy.
    /// </summary>
    [Header("Material settings")]
    [SerializeField] private Color healthyColor;
    /// <summary>
    /// Color the refugee takes when unhealthy.
    /// </summary>
    [SerializeField] private Color unhealthyColor;

    /// <summary>
    /// Reference to the refugee's material.
    /// </summary>
    private Material myMat;

    /// <summary>
    /// Number of Ticks to wait between updating stat values.
    /// </summary>
    [Header("Stat UpdateSettings")]
    [Tooltip("Number of Ticks to wait between updating stat values.")]
    [SerializeField] private float updateStatTimer = 2f;
    /// <summary>
    /// Amount to decrease shelter by, if decrease is required. 
    ///</summary>
    [Tooltip("Amount to decrease shelter by, if decrease is required.")]
    [SerializeField] private int shelterDecrease = 1;
    /// <summary>
    /// Amount to increase shelter by, if decrease is required. 
    ///</summary>
    [Tooltip("Amount to increase shelter by, if decrease is required.")]
    [SerializeField] private int shelterIncrease = 1;
    /// <summary>
    /// Amount to decrease hydration by, if decrease is required. 
    ///</summary>
	[Tooltip("Amount to decrease hydration by, if decrease is required.")]
    [SerializeField] private int hydrationDecrease = 1;
    /// <summary>
	/// Amount to increase hydration by, if decrease is required. 
	///</summary>
	[Tooltip("Amount to increase hydration by, if decrease is required.")]
    [SerializeField] private int hydrationIncrease = 1;
    /// <summary>
	/// Amount to decrease food by, if decrease is required. 
	///</summary>
	[Tooltip("Amount to decrease food by, if decrease is required.")]
    [SerializeField] private int foodDecrease = 1;
    /// <summary>
	/// Amount to increase food by, if decrease is required. 
	///</summary>
	[Tooltip("Amount to increase food by, if decrease is required.")]
    [SerializeField] private int foodIncrease = 1;
    /// <summary>
	/// Amount to decrease hygiene by, if decrease is required. 
	///</summary>
	[Tooltip("Amount to decrease hygiene by, if decrease is required.")]
    [SerializeField] private int hygieneDecrease = 1;
    /// <summary>
	/// Amount to increase hygiene by, if decrease is required. 
	///</summary>
	[Tooltip("Amount to increase hygiene by, if decrease is required.")]
    [SerializeField] private int hygieneIncrease = 1;


    private float currTime = 0;

    #endregion

    #region GettersSetters
    /// <summary>
    /// Accessor for shelter value
    /// </summary>
    public Shelter GetHealth()
    {
        return shelter;
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
    /// Sets up initial references to transform, shelter, hydration, food, hygiene and well being. As well as subscribes to events.
    /// </summary>
    private void Start()
    {
        cachedTransform = transform;
        shelter = GetComponent<Shelter>();
        hydration = GetComponent<Hydration>();
        food = GetComponent<Food>();
        hygiene = GetComponent<Hygiene>();
        wellbeing = GetComponent<OverallWellbeing>();

        myMat = GetComponent<Renderer>().material;

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
        currTime += TimeManager.GetInstance().GetDeltaTime();
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
    /// Updates the refugee shelter stat
    /// </summary>
    private void UpdateHealth()
    {
        if(house == null || house.GetType() == typeof(HomelessHouse))
        {
            shelter.SubtractCurrStat(shelterDecrease);
        }
        else
        {
            shelter.SetMaxStat(house.GetHealth().GetCurrStatVal());
            if (shelter.GetMaxStatVal() < shelter.GetCurrStatVal())
            {
                shelter.SubtractCurrStat(shelterDecrease);
            }
            else
            {
                shelter.AddCurrStat(shelterIncrease);
            }
        }
    }

    /// <summary>
    /// Updates the refugee hydration stat
    /// </summary>
    private void UpdateHydration()
    {
        if (house == null) //We don't want to subtract shelter from registered homeless currently.
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
        wellbeing.SetCurrStat(shelter.GetCurrStatVal(), hydration.GetCurrStatVal());
        myMat.SetColor("_BaseColor", Color.Lerp(unhealthyColor, healthyColor, wellbeing.GetCurrStatVal()*1f / wellbeing.GetMaxStatVal()));

        if(wellbeing.GetCurrStatVal() == wellbeing.GetMinStatVal())
        {
            OnDiedEventHandler(this); //Calling this for now because I know Tejas wrote code that utilizes this event handler.
        }
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

