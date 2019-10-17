using UnityEngine;

using Sirenix.OdinInspector;

public class MeasureModeManager : Singleton<MeasureModeManager>
{
	#region Variables
	[SerializeField, Tooltip("Whether Measure Mode is enabled")]
	bool measureMode = false;
	#endregion Variables

	#region Getters & Setters
	/// <summary>
	/// Get whether currently in Measure Mode
	/// </summary>
	/// <returns>True if in Measure Mode and false otherwise</returns>
	public bool GetMeasureMode()
	{
		return measureMode;
	}

	/// <summary>
	/// Set whether in Measure Mode
	/// </summary>
	/// <param name="_measureMode">Whether Measure Mode should be enabled</param>
	public void SetMeasureMode(bool _measureMode)
	{
		if (measureMode != _measureMode)
		{
			measureMode = _measureMode;

			OnMeasureModeChange?.Invoke(measureMode);
		}
	}
	#endregion Getters & Setters

	#region Events
	/// <summary>
	/// Handler for event invoked when Measure Mode is toggled on or off
	/// </summary>
	/// <param name="_measureMode">Whether Measure Mode is on</param>
	public delegate void MeasureModeChangeEventHandler(bool _measureMode);
	/// <summary>
	/// Event invoked when Measure Mode is toggled on or off
	/// </summary>
	public event MeasureModeChangeEventHandler OnMeasureModeChange;
	#endregion Events

	#region Public Methods
	/// <summary>
	/// Toggle whether Measure Mode is enabled
	/// </summary>
	public void ToggleMeasureMode()
	{
		SetMeasureMode(!measureMode);
	}
	#endregion Public Methods
}