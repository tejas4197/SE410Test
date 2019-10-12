using UnityEngine;

public class TriggerAwake : EventTrigger
{
	#region MonoBehaviour
	private void Awake()
	{
		InvokeEvent();
	}
	#endregion MonoBehaviour
}