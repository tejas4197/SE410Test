using UnityEngine;

public class TriggerSceneStart : EventTrigger
{
	#region MonoBehaviour
	private void Start()
	{
		InvokeEvent();
	}
	#endregion MonoBehaviour
}