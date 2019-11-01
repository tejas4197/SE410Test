using UnityEngine;

using Sirenix.OdinInspector;

/// <summary>
/// Event Action invoked at scene start based on whether Sky View is enabled
/// </summary>
public class TriggerStartSkyViewSetting : EventTrigger
{
	#region Variables
	[SerializeField, Tooltip("Invoke the event when Sky View is set to this value")]
	bool skyViewSetting = true;
	#endregion Variables

	#region MonoBehaviour
	private void Start()
	{
		if(MeasureModeManager.GetInstance().GetMeasureMode() == skyViewSetting)
		{
			InvokeEvent();
		}
	}
	#endregion MonoBehaviour
}