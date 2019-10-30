using System;
using System.Collections;

using UnityEngine;

using Sirenix.OdinInspector;

public abstract class EventTrigger : MonoBehaviour
{
	#region Variables
	[ReadOnly]
	[SerializeField, Tooltip("Whether this trigger has been fired already")]
	private bool hasFired = false;

	[SerializeField, Tooltip("Whether this trigger can be invoked multiple times")]
	private bool repeatable = false;

	[SerializeField, Tooltip("The next EventTrigger to enable, if any")]
	private EventTrigger nextTrigger;

	[ReadOnly, ValidateInput("ActionsDeclared")]
	[SerializeField, Tooltip("All the Actions that will run with this trigger")]
	private EventAction[] actions;
	#endregion Variables

	#region Getters & Setters
	/// <summary>
	/// Whether this trigger has been fired already
	/// </summary>
	public bool GetHasFired()
	{
		return hasFired;
	}

	/// <summary>
	/// Whether this trigger can be invoked multiple times
	/// </summary>
	public bool GetRepeatable()
	{
		return repeatable;
	}
	#endregion Getters & Setters

	#region Events
	public delegate void InvokeEventHandler(EventTrigger _trigger);
	public event InvokeEventHandler OnInvoke;
	#endregion Events

	#region MonoBehaviour
	private void Awake()
	{
		GetActions();
		CustomAwake();
	}

	private void OnEnable()
	{
		hasFired = false;
		CustomOnEnable();
	}

	private void OnDisable()
	{
		CustomOnDisable();
	}
	#endregion MonoBehaviour

	#region Public Methods
	public void GetActions()
	{
		actions = GetComponents<EventAction>();
		Array.Sort(actions, delegate (EventAction ea1, EventAction ea2) { return ea1.GetPriority().CompareTo(ea2.GetPriority()); });
	}
	#endregion Public Methods

	#region Protected Methods
	virtual protected void CustomAwake() { }

	virtual protected void CustomOnEnable() { }

	virtual protected void CustomOnDisable() { }

	/// <summary>
	/// The function that child objects must call to invoke the event
	/// </summary>
	[Button("Fire Trigger", ButtonSizes.Medium)]
	protected void InvokeEvent()
	{
		// First ensure the event hasn't fired
		// This is so the trigger doesn't fire again while already firing
		if (!hasFired)
		{
			hasFired = true;

			if (nextTrigger != null)
			{
				// Enable the next trigger before executing Event Actions
				// Do it before actions in case the next trigger listens for the actions to be executed
				nextTrigger.gameObject.SetActive(true);
			}

			foreach (EventAction action in actions)
			{
				if (action.enabled)
				{
					//Debug.Log("[EventTrigger] Trigger is " + gameObject + "\nAction invoking now is: " + action);
					action.StartAction();
				}
			}

			if (gameObject.activeInHierarchy)
			{
				StartCoroutine("AfterFire");
			}
		}
	}
	#endregion Protected Methods

	#region Private Methods
	/// <summary>
	/// Cleanup after the trigger has been invoked
	/// </summary>
	private IEnumerator AfterFire()
	{
		// Ensure all actions have completed before continuing
		foreach (EventAction action in actions)
		{
			if (action.enabled)
			{
				while (!action.GetActionDone())
				{
					yield return null;
				}
			}
		}

		if (OnInvoke != null)
		{
			OnInvoke(this);
		}

		if (repeatable)
		{
			hasFired = false;

			foreach (EventAction action in actions)
			{
				action.ResetAction();
			}

			CustomOnEnable();
		}
		else
		{
			gameObject.SetActive(false);
		}
	}
	#endregion Private Methods

	#region Odin Validation
	private bool ActionsDeclared(EventAction[] _actions)
	{
		return _actions != null;
	}
	#endregion Odin Validation
}