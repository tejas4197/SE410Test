using System.Collections;

using UnityEngine;

using Sirenix.OdinInspector;

public class TriggerGameObjectEnabled : EventTrigger
{
	#region Variables
	[Required]
	[SerializeField, Tooltip("The GameObject to wait for disabling on")]
	private GameObject targetGameObject;
	#endregion Variables

	#region Protected Methods
	protected override void CustomOnEnable()
	{
		StartCoroutine("CheckActive");
	}

	protected override void CustomOnDisable()
	{
		StopAllCoroutines();
	}
	#endregion Protected Methods

	#region Private Methods
	private IEnumerator CheckInactive()
	{
		while (!targetGameObject.activeInHierarchy)
		{
			yield return null;
		}

		InvokeEvent();
	}

	private IEnumerator CheckActive()
	{
		while (targetGameObject.activeInHierarchy)
		{
			yield return null;
		}

		StartCoroutine("CheckInactive");
	}
	#endregion Private Methods
}