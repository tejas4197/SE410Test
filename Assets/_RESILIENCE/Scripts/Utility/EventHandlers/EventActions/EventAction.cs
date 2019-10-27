using System.Collections;

using UnityEngine;

using Sirenix.OdinInspector;

public abstract class EventAction : MonoBehaviour
{
	#region Variables
	[SerializeField, Tooltip("The priority of this EventAction (0 is default, less than 0 executes before the default time and greater than 0 after)")]
	private int priority = 0;

	// Whether this action has been performed
	private bool actionDone = false;

	[MinValue(0f)]
	[SerializeField, Tooltip("The amount of time to wait before the action is carried out")]
	private protected float waitTime = 0f;
	#endregion Variables

	#region Getters & Setters
	/// <summary>
	/// The priority of this EventAction (0 is default, less than 0 executes before the default time and greater than 0 after)
	/// </summary>
	/// <returns>Returns the priority number of this EventAction (less than 0 executes before default and greater than 0 is after)</returns>
	public int GetPriority()
	{
		return priority;
	}

	/// <summary>
	/// Whether this action has been performed
	/// </summary>
	/// <returns>Bool representing whether this action has been completed</returns>
	public bool GetActionDone()
	{
		return actionDone;
	}

	/// <summary>
	/// Set whether the action has been completed (can be overriden for classes that allow explicit action completion)
	/// </summary>
	/// <param name="_actionDone">Whether the action has been completed</param>
	protected virtual void SetActionDone(bool _actionDone)
	{
		actionDone = _actionDone;
	}
	#endregion Getters & Setters

	#region MonoBehaviour
	private void OnEnable()
	{
		ResetAction();

		CustomOnEnable();
	}
	#endregion MonoBehaviour

	#region Public Methods
	/// <summary>
	/// Start the timer before executing the action
	/// </summary>
	public void StartAction()
	{
		if (gameObject.activeInHierarchy)
		{
			StartCoroutine("ActionTimer");
		}
		else
		{
			ExecuteAction();

			SetActionDone(true);
		}
	}

	/// <summary>
	/// Reset this action's complete status, allowing it to fire again
	/// </summary>
	public void ResetAction()
	{
		actionDone = false;
	}
	#endregion Public Methods

	#region Protected Methods
	/// <summary>
	/// Custom OnEnable function to be overriden in child classes to prevent hiding of base OnEnable
	/// </summary>
	virtual protected void CustomOnEnable() { }

	/// <summary>
	/// The timer before executing an action
	/// </summary>
	protected IEnumerator ActionTimer()
	{
		yield return new WaitForTicks(waitTime);

		ExecuteAction();

		SetActionDone(true);
	}

	/// <summary>
	/// Execute the action when the trigger has fired
	/// </summary>
	protected abstract void ExecuteAction();
	#endregion Protected Methods
}