using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

public class RandomEvent : MonoBehaviour
{
	#region Variables
	[ValidateInput("StringNotEmpty", "The name of this event cannot be empty")]
	[SerializeField, Tooltip("The name of the random event")]
	string eventName;

	[MinValue(0), ValidateInput("GreaterThanZero", "This event will not be able to happen if its chance is 0")]
	[SerializeField, Tooltip("The likelihood of this event occurring (must be non-zero to be possible)")]
	int chance = 0;

	[ReadOnly]
	[SerializeField, Tooltip("The list of all effects to run when the event is invoked")]
	RandomEventEffect[] effects;

	[ReadOnly]
	[SerializeField, Tooltip("The effects not yet completed from this event")]
	List<RandomEventEffect> effectsNotCompleted = new List<RandomEventEffect>();
	#endregion Variables

	#region Getters & Setters
	/// <summary>
	/// Get the name of the random event
	/// </summary>
	/// <returns>The string name of the event</returns>
	public string GetEventName()
	{
		return eventName;
	}

	/// <summary>
	/// Get the likelihood of this event occurring (must be non-zero to be possible)
	/// </summary>
	/// <returns>The likelihood of this event occurring (will be integer value greater than or equal to 0)</returns>
	public int GetChance()
	{
		return chance;
	}
	#endregion Getters & Setters

	#region Events
	/// <summary>
	/// Handler for events related to this RandomEvent
	/// </summary>
	/// <param name="_event">The RandomEvent invoking this event</param>
	public delegate void EventHandler(RandomEvent _event);

	/// <summary>
	/// Event invoked when the event has fully finished, meaning all effects have been carried out completely
	/// </summary>
	public event EventHandler OnEventEnd;
	#endregion Events

	#region MonoBehaviour
	private void Awake()
	{
		FindEffects();
	}
	#endregion MonoBehaviour

	#region Public Methods
	/// <summary>
	/// Run all the effects as part of this event
	/// </summary>
	public void Run()
	{
		effectsNotCompleted.Clear();

		// Run all effects and add them to the list of effects that have not yet been completed
		foreach (RandomEventEffect effect in effects)
		{
			// if the effect reference is null (which shouldn't happen if all references are set properly in the inspector),
			// Then skip this effect and move to the next one
			if(effect == null)
			{
				continue;
			}

			effectsNotCompleted.Add(effect);
			effect.OnEffectEnd += MarkEffectCompleted;
			effect.StartAction();
		}
	}

	public override string ToString()
	{
		return eventName;
	}
	#endregion Public Methods

	#region Private Methods
	/// <summary>
	/// Find all effects attached to the random event
	/// </summary>
	public void FindEffects()
	{
		effects = GetComponents<RandomEventEffect>();
		System.Array.Sort(effects, delegate (RandomEventEffect ree1, RandomEventEffect ree2) { return ree1.GetPriority().CompareTo(ree2.GetPriority()); });
	}

	/// <summary>
	/// Mark the effect as completed and check if there are still more effects being invoked (called by the _effect's OnEffectEnd event)
	/// </summary>
	/// <param name="_effect">The RandomEventEffect that has been completed</param>
	private void MarkEffectCompleted(RandomEventEffect _effect)
	{
		if(_effect == null)
		{
			return;
		}

		if (effectsNotCompleted.Contains(_effect))
		{
			effectsNotCompleted.Remove(_effect);
		}

		if (effectsNotCompleted.Count == 0)
		{
			InvokeEventEnd();
		}
	}

	/// <summary>
	/// Invoke the OnEventEnd event (called in MarkEffectCompleted() if effectsNotCompleted list is empty after an effect has finished)
	/// </summary>
	private void InvokeEventEnd()
	{
		OnEventEnd?.Invoke(this);
	}
	#endregion Private Methods

	#region Odin Validation
	/// <summary>
	/// Check that a string of text is not empty
	/// </summary>
	/// <param name="_text">The string to check</param>
	/// <returns>True if the input string is not null or empty and false otherwise</returns>
	private bool StringNotEmpty(string _text)
	{
		return !string.IsNullOrEmpty(_text);
	}

	/// <summary>
	/// Check that an int is greater than 0
	/// </summary>
	/// <param name="_num">The number to check</param>
	/// <returns>True if the number is greater than 0 and false otherwise</returns>
	private bool GreaterThanZero(int _num)
	{
		return _num > 0;
	}

	/// <summary>
	/// Check that a list of RandomEventEffects is not null or empty
	/// </summary>
	/// <param name="_effects">The list to check for a non-zero count</param>
	/// <returns>True if the input list is not null or empty and false otherwise</returns>
	private bool ListNotEmpty(List<RandomEventEffect> _effects)
	{
		return _effects != null && _effects.Count > 0;
	}

	/// <summary>
	/// Check that a list of RandomEventEffects does not contain any nulls
	/// </summary>
	/// <param name="_effects">The list to check for no null references</param>
	/// <returns>True if no null references are found and false otherwise</returns>
	private bool ListContainsNoNulls(List<RandomEventEffect> _effects)
	{
		foreach(RandomEventEffect effect in _effects)
		{
			if(effect == null)
			{
				return false;
			}
		}

		return true;
	}
	#endregion Odin Validation

}