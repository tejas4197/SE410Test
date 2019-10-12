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
		float timeElapsed = 0f;

		while (timeElapsed < waitTime)
		{
			yield return null;

			// Only increment the timer if the game isn't paused
			if (!PauseManager.GetInstance().GetIsPaused())
			{
				timeElapsed += Time.deltaTime;
			}
		}

		InvokeEvent();
	}
	#endregion Private Methods
}