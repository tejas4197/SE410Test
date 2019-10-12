using UnityEngine;

using Sirenix.OdinInspector;

public class TriggerOnEnable : EventTrigger
{
	#region Protected Methods
	protected override void CustomOnEnable()
	{
		InvokeEvent();
	}
	#endregion Protected Methods
}