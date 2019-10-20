using UnityEngine;

using Sirenix.OdinInspector;

/// <summary>
/// Event Trigger invoked when Sky View is toggled and optionally set to a specific status
/// </summary>
public class TriggerSkyViewToggle : EventTrigger
{
	#region Variables
	[SerializeField, Tooltip("Whether to check the current Sky View mode (if false, invoke regardless of whether Sky View is enabled or disabled)")]
	bool checkSkyViewSetting = true;

	[ShowIf("checkSkyViewSetting")]
	[SerializeField, Tooltip("Invoke the event when Sky View is set to this value")]
	bool skyViewSetting = true;
	#endregion Variables

	#region Protected Methods
	protected override void CustomOnEnable()
	{
		MeasureModeManager.GetInstance().OnMeasureModeChange += CheckMode;
	}

	protected override void CustomOnDisable()
	{
		MeasureModeManager.GetInstance().OnMeasureModeChange -= CheckMode;
	}
	#endregion Protected Methods

	#region Private Methods
	/// <summary>
	/// Compare the current Sky View setting (enabled or not) to the target setting for invoking the event
	/// </summary>
	/// <param name="_skyView">Sky View's current status (either enabled or not)</param>
	private void CheckMode(bool _skyView)
	{
		if(!checkSkyViewSetting || skyViewSetting == _skyView)
		{
			InvokeEvent();
		}
	}
	#endregion Private Methods
}