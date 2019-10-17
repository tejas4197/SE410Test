using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

public class TriggerGroup : EventTrigger
{
	#region Variables
	[ReadOnly]
	[SerializeField, Tooltip("The triggers within this group")]
	private EventTrigger[] childrenTriggers;

	private List<EventTrigger> childrenLeft;

	[SerializeField, Tooltip("Whether all children must be complete to continue (if false, will proceed after the first is complete)")]
	private bool completeAllChildren = true;
	#endregion Variables

	#region MonoBehaviour
	private void Start()
	{
		childrenTriggers = GetComponentsInChildren<EventTrigger>(true);
	}
	#endregion MonoBehaviour

	#region Protected Methods
	protected override void CustomOnEnable()
	{
		// Reset list of the children that still have not fired
		childrenLeft = new List<EventTrigger>();

		foreach (EventTrigger trigger in childrenTriggers)
		{
			trigger.gameObject.SetActive(true);

			childrenLeft.Add(trigger);

			trigger.OnInvoke += CheckComplete;
		}
	}
	#endregion Protected Methods

	#region Private Methods
	/// <summary>
	/// Check if ready to invoke this group's event (all or at least one child has completed)
	/// </summary>
	/// <param name="_triggerFired">The child trigger that has fired</param>
	private void CheckComplete(EventTrigger _triggerFired)
	{
		if (completeAllChildren)
		{
			childrenLeft.Remove(_triggerFired);

			if (childrenLeft.Count == 0)
			{
				InvokeEvent();
			}
		}
		else
		{
			InvokeEvent();
		}
	}
	#endregion Private Methods
}