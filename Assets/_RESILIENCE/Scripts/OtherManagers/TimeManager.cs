using System.Collections;

using UnityEngine;

using Sirenix.OdinInspector;

public class TimeManager : Singleton<TimeManager>
{
	#region VARIABLES
	[ReadOnly, MinValue(1)]
	[SerializeField, Tooltip("The current month in the game")]
	int currentMonth = 1;

	[Header("Time Passage")]
	[OnValueChanged("EditorUpdateIsTimePassing")]
	[SerializeField, Tooltip("Whether time should be passing right now")]
	bool isTimePassing = true;

	[MinValue(0f)]
	[SerializeField, Tooltip("The amount of time in seconds that it takes for a month to pass")]
	float secondsPerMonth = 120f;

	[MinValue(0f)]
	[SerializeField, Tooltip("The multiplier to time passing (0 means time is paused, 2 would mean time passes twice as quickly)")]
	float timePassageMultiplier = 1f;

	[ReadOnly]
	[SerializeField, Tooltip("The amount of time that has passed since the start of this month")]
	float timePassedThisMonth = 0f;

	[ReadOnly]
	[SerializeField, Tooltip("The amount of time that has passed since the last tick")]
	float deltaTime = 0f;
	#endregion VARIABLES

	#region GETTERS_SETTERS
	/// <summary>
	/// Accessor for the current month
	/// </summary>
	public int GetCurrentMonth()
	{
		return currentMonth;
	}

	/// <summary>
	/// Mutator for the current month, which also calls the OnMonthPass event
	/// </summary>
	private void SetCurrentMonth(int _currentMonth)
	{
		if (currentMonth != _currentMonth && _currentMonth > 0)
		{
			currentMonth = _currentMonth;

			OnMonthPass?.Invoke();
		}
	}

	/// <summary>
	/// Get whether time is passing now
	/// </summary>
	public bool GetIsTimePassing()
	{
		return isTimePassing;
	}

	/// <summary>
	/// Change whether time should be passing now
	/// </summary>
	public void SetIsTimePassing(bool _isTimePassing)
	{
		if (isTimePassing != _isTimePassing)
		{
			isTimePassing = _isTimePassing;

			if (isTimePassing)
			{
				StartCoroutine("TimePasser");
			}
			else
			{
				StopCoroutine("TimePasser");
			}

			OnIsTimePassingChange?.Invoke();
		}
	}

	/// <summary>
	/// Accessor for the timePassageMultiplier (the multiplier to time passing, with 0 meaning time is paused and 2 doubling how fast time goes)
	/// </summary>
	public float GetTimePassageMultiplier()
	{
		return timePassageMultiplier;
	}

	/// <summary>
	/// Mutator for the timePassageMultiplier (the multiplier to time passing, with 0 meaning time is paused and 2 doubling how fast time goes)
	/// </summary>
	public void SetTimePassageMultiplier(float _timePassageMultiplier)
	{
		if (timePassageMultiplier != _timePassageMultiplier && _timePassageMultiplier >= 0)
		{
			timePassageMultiplier = _timePassageMultiplier;

			OnTimePassageMultiplierChange?.Invoke();
		}
	}

	/// <summary>
	/// Accessor for the timePassed variable, tracking how much time in seconds has passed since the start of the month
	/// </summary>
	/// <returns>Float value greater than or equal to 0 of how much time in seconds has passed</returns>
	public float GetTimePassedThisMonth()
	{
		return timePassedThisMonth;
	}

	private void SetTimePassedThisMonth(float _timePassedThisMonth)
	{
		if (_timePassedThisMonth >= 0 && timePassedThisMonth != _timePassedThisMonth)
		{
			timePassedThisMonth = _timePassedThisMonth;

			OnTick?.Invoke();
		}
	}

	/// <summary>
	/// Accessor for the amount of time that has passed since the last tick, similar to Time.deltaTime
	/// </summary>
	/// <returns>Float value greater than or equal to 0</returns>
	public float GetDeltaTime()
	{
		return deltaTime;
	}
	#endregion GETTERS_SETTERS

	#region EVENTS
	/// <summary>
	/// Delegate for subscribing to the OnTick event
	/// </summary>
	public delegate void TickHandler();
	/// <summary>
	/// Event invoked whenever a tick occurs, moving time forward (which happens every frame that the game isn't paused)
	/// </summary>
	public event TickHandler OnTick;

	/// <summary>
	/// Delegate for subscribing to the OnMonthPass event
	/// </summary>
	public delegate void MonthPassHandler();
	/// <summary>
	/// Event invoked when a month has passed and a new one has begun
	/// </summary>
	public event MonthPassHandler OnMonthPass;

	/// <summary>
	/// Delegate for subscribing to the OnIsTimePassingChange event
	/// </summary>
	public delegate void IsTimePassingChangeHandler();
	/// <summary>
	/// Event invoked when whether time is passing is changed
	/// </summary>
	public event IsTimePassingChangeHandler OnIsTimePassingChange;

	/// <summary>
	/// Delegate for subscribing to the OnTimePassageMultiplierChange event
	/// </summary>
	public delegate void TimePassageMultiplierChangeHandler();
	/// <summary>
	/// Event invoked when the TimePassageMutliplier value is changed
	/// </summary>
	public event TimePassageMultiplierChangeHandler OnTimePassageMultiplierChange;
	#endregion EVENTS

	#region MONOBEHAVIOUR
	private void Start()
	{
		if (isTimePassing)
		{
			StartCoroutine("TimePasser");
		}
	}

    private void OnEnable()
    {
        DebugConsole.GetInstance().SubmitCommand += DebugTimeMultiManipulate;
    }

    private void OnDisable()
    {
        DebugConsole.GetInstance().SubmitCommand -= DebugTimeMultiManipulate;
    }

    #endregion MONOBEHAVIOUR

    #region PUBLIC_METHODS

    #endregion PUBLIC_METHODS

    #region PRIVATE_METHODS

    /// <summary>
    /// Debug command to change how fast time moves
    /// </summary>
    public void DebugTimeMultiManipulate(string command)
    {
        string[] parsedCommand = command.Split(' ');
        float multipler = 0.0f;

        if (parsedCommand.Length == 2 && parsedCommand[0].ToLower() == "time" && float.TryParse(parsedCommand[1], out multipler))
        {
            timePassageMultiplier = multipler;
            Debug.Log("Time Rate is now " + timePassageMultiplier);
        }
    }

    /// <summary>
    /// Coroutine handling time passage loop
    /// </summary>
    private IEnumerator TimePasser()
	{
		timePassedThisMonth = 0;

		while (timePassedThisMonth < secondsPerMonth)
		{
			yield return null;

			if (isTimePassing)
			{
				deltaTime = UnityEngine.Time.deltaTime * timePassageMultiplier;

				SetTimePassedThisMonth(timePassedThisMonth + deltaTime);
			}
			else
			{
				deltaTime = 0f;
			}
		}

		IncrementMonth();

		StartCoroutine("TimePasser");
	}

	/// <summary>
	/// Pass forward one month
	/// </summary>
	private void IncrementMonth()
	{
		SetCurrentMonth(currentMonth + 1);
	}

	/// <summary>
	/// Editor-specific function called by Odin when the IsTimePassing value is changed in Unity Inspector to start or cancel the TimePassing coroutine
	/// </summary>
	private void EditorUpdateIsTimePassing()
	{
		if (Application.isPlaying)
		{
			if (isTimePassing)
			{
				StartCoroutine("TimePasser");
			}
			else
			{
				StopCoroutine("TimePasser");
			}

			OnIsTimePassingChange?.Invoke();
		}
	}
	#endregion PRIVATE_METHODS
}