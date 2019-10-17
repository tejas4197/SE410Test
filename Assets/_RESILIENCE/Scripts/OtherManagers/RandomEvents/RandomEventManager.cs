using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

[System.Serializable]
public class RandomEventManager : Singleton<RandomEventManager>
{
	#region Variables
	[OnValueChanged("UpdateTotalChance", true)]
	[SerializeField, Tooltip("The different possible random events that can occur")]
	List<RandomEvent> events = new List<RandomEvent>();

	[ReadOnly]
	[SerializeField, Tooltip("The sum of chance values for all events possible (used for determining which event to cue when one is starting)")]
	int totalChance;

	[ReadOnly]
	[SerializeField, Tooltip("The event currently running")]
	RandomEvent currentEvent = null;

	[Header("Current Chance Tracking")]
	
	[ReadOnly, MinValue(0), MaxValue(100)]
	[SerializeField, Tooltip("The current chance of an event occurring on a scale of [0, 100] (0 is impossible, 100 is guaranteed)")]
	int currentChance = 0;

	[MinValue(0), MaxValue(100)]
	[SerializeField, Tooltip("The minimum chance of an event occurring, which currentChance will be reset to whenever an event is triggered")]
	int minChance = 0;

	[MinValue(0), MaxValue(100)]
	[SerializeField, Tooltip("The maximum chance currentChance can be set to on a scale of [0, 100]")]
	int maxChance = 100;

	[ReadOnly]
	[SerializeField, Tooltip("Whether an event is currently occurring")]
	bool isEventOccurring = false;

	[Header("Chance Ticking")]

	[SerializeField, Tooltip("Whether to run event checks every frame")]
	bool tickEveryFrame = true;

	[HideIf("tickEveryFrame")]
	[MinValue(0f)]
	[SerializeField, Tooltip("The amount of time in seconds that should pass between ticks if not running every frame")]
	float tickTime = 1f;

	[MinValue(1)]
	[SerializeField, Tooltip("The amount to change the current chance with each tick")]
	int deltaChance = 1;

	[ReadOnly]
	[SerializeField, Tooltip("The amount of time that has passed since the last tick")]
	float tickTimePassed = 0f;
	#endregion Variables

	#region Getters & Setters
	/// <summary>
	/// The event currently running (or null if none is running)
	/// </summary>
	public RandomEvent GetCurrentEvent()
	{
		return currentEvent;
	}

	/// <summary>
	/// Set the event currently running (or null if none is running)
	/// </summary>
	/// <param name="_randomEvent">The new RandomEvent to treat as the current one</param>
	private void SetCurrentEvent(RandomEvent _randomEvent)
	{
		if (currentEvent != null)
		{
			currentEvent.OnEventEnd -= TriggerEventEnd;
		}

		currentEvent = _randomEvent;

		if (currentEvent != null)
		{
			Debug.Log($"[RandomEventManager] Starting new event: {currentEvent.ToString()}");
		}

		if (currentEvent != null)
		{
			currentEvent.OnEventEnd += TriggerEventEnd;
		}
	}
	#endregion Getters & Setters

	#region Events
	public delegate void EventHandler(RandomEvent _event);

	/// <summary>
	/// Event called when a RandomEvent is about to start
	/// </summary>
	public event EventHandler OnEventStart;

	/// <summary>
	/// Event called when a RandomEvent is ending
	/// </summary>
	public event EventHandler OnEventEnd;
	#endregion Events

	#region MonoBehaviour
	private void OnEnable()
	{
		TimeManager.GetInstance().OnTick += EventTick;
	}

	private void OnDisable()
	{
		TimeManager.GetInstance().OnTick -= EventTick;
	}
	#endregion MonoBehaviour

	#region Private Methods
	/// <summary>
	/// Run the "tick" loop, checking at each update whether a random event should occur and, if not, increment the chance of an event occurring
	/// </summary>
	private void EventTick()
	{
		if (!isEventOccurring)
		{
			tickTimePassed = tickTimePassed + TimeManager.GetInstance().GetDeltaTime();

			if (tickEveryFrame || tickTimePassed >= tickTime)
			{
				// Requires a non-zero chance for a random event to occur
				if (currentChance > 0)
				{
					int chance = Random.Range(0, 100);

					Debug.Log($"[RandomEventManager] Chance tick. Chance of event is {currentChance} and random number is {chance}.");

					if (chance < currentChance)
					{
						Debug.Log($"[RandomEventManager] Generated number {chance} is less than current chance {currentChance}, so starting event.");
						TriggerEvent();
					}
				}

				// No event is occurring, so increase the chance of it occurring later
				if (!isEventOccurring)
				{
					// Increase currentChance without exceeding the max chance allowed
					currentChance = Mathf.FloorToInt(Mathf.Clamp(currentChance + deltaChance, minChance, maxChance));
				}

				tickTimePassed = 0f;
			}
		}
	}

	/// <summary>
	/// Choose an event to run and then start running it
	/// </summary>
	private void TriggerEvent()
	{
		isEventOccurring = true;

		int eventAmount = Random.Range(0, maxChance);

		foreach (RandomEvent randEvent in events)
		{
			if (eventAmount >= 0)
			{
				eventAmount -= randEvent.GetChance();

				if (eventAmount <= 0)
				{
					SetCurrentEvent(randEvent);

					OnEventEnd?.Invoke(currentEvent);

					currentEvent.Run();
				}
			}
		}
	}

	/// <summary>
	/// Trigger the end of an event and restart the event-tick process
	/// </summary>
	private void TriggerEventEnd(RandomEvent _event)
	{
		OnEventEnd?.Invoke(_event);

		SetCurrentEvent(null);

		isEventOccurring = false;

		// Reset chance to the lowest possible for starting out with next event
		currentChance = minChance;
	}
	#endregion Private Methods

	#region Odin Validation
	/// <summary>
	/// Update the totalChance value to 
	/// </summary>
	private void UpdateTotalChance()
	{
		totalChance = 0;

		foreach (RandomEvent randEvent in events)
		{
			totalChance += randEvent.GetChance();
		}
	}
	#endregion Odin Validation
}