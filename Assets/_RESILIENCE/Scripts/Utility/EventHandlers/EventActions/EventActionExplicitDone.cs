using UnityEngine;

using Sirenix.OdinInspector;

public abstract class EventActionExplicitDone : EventAction
{
	#region Variables
	[Header("Debug")]
	[SerializeField, Tooltip("Whether to show debug messages")]
	private bool showDebug = false;
	#endregion Variables

	#region Getters & Setters
	/// <summary>
	/// Ignore input here so we can explicitly say when the action is done
	/// </summary>
	/// <param name="_actionDone">Whether the action has been completed (passed in automatically, so will be ignored)</param>
	protected override void SetActionDone(bool _actionDone)
	{
		/* Do nothing */
		if (showDebug)
		{
			Debug.Log($"[EventActionExplicitDone] Trying to set actionDone to {_actionDone}");
		}
	}
	#endregion Getters & Setters

	#region Protected Methods
	protected void CompleteAction()
	{
		base.SetActionDone(true);
	}
	#endregion Protected Methods
}