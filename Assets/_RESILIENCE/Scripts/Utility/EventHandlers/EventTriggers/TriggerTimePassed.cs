using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

public class TriggerTimePassed : EventTrigger
{
	#region Variables
	[MinValue(0f)]
	[SerializeField, Tooltip("The amount of time to wait before firing")]
	private float waitTime = 3f;

	private Coroutine timerRoutine;
	#endregion Variables

	#region Protected Methods
	protected override void CustomOnEnable()
	{
		timerRoutine = StartCoroutine("Timer");
	}

	protected override void CustomOnDisable()
	{
		StopCoroutine(timerRoutine);
	}
	#endregion Protected Methods

	#region Private Methods
	/// <summary>
	/// Run a timer for waitTime seconds before invoking the actions
	/// </summary>
	private IEnumerator Timer()
	{
		yield return new WaitForTicks(waitTime);

		if (gameObject.activeInHierarchy && enabled)
		{
			InvokeEvent();
		}
	}
	#endregion Private Methods
}