using System.Collections;

using UnityEngine;

using Sirenix.OdinInspector;

public class TriggerGameObjectDisabled : EventTrigger
{
	#region Variables
	[Required]
	[SerializeField, Tooltip("The GameObject to wait for disabling on")]
	private GameObject targetGameObject;
	#endregion Variables

	#region Protected Methods
	protected override void CustomOnEnable()
	{
		StartCoroutine("CheckInactive");
	}

	protected override void CustomOnDisable()
	{
		StopAllCoroutines();
	}
	#endregion Protected Methods

	#region Private Methods
	/// <summary>
	/// Monitor the GameObject while it's inactive until it has been enabled (that way, the object has to be disabled and can't just start out disabled)
	/// </summary>
	private IEnumerator CheckInactive()
	{
		while (!targetGameObject.activeInHierarchy)
		{
			yield return null;
		}

		StartCoroutine("CheckActive");
	}

	/// <summary>
	/// Monitor the active GameObject until it is deactivated
	/// </summary>
	private IEnumerator CheckActive()
	{
		while (targetGameObject.activeInHierarchy)
		{
			yield return null;
		}

		InvokeEvent();
	}
	#endregion Private Methods
}