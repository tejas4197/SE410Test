using UnityEngine;

using Sirenix.OdinInspector;

public class TriggerOnDisable : EventTrigger
{
	#region Protected Methods
	protected override void CustomOnDisable()
	{
		InvokeEvent();
	}
	#endregion Protected Methods
}