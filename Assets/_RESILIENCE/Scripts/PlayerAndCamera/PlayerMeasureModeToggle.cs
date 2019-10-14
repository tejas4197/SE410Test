using UnityEngine;

using Sirenix.OdinInspector;

using Rewired;

/// <summary>
/// Class for toggling between Sky View and Ground View with player input
/// </summary>
public class PlayerMeasureModeToggle : MonoBehaviour
{
	#region Variables
	[Header("Input")]

	[Required]
	[SerializeField, Tooltip("The script for monitoring Rewired player input")]
	private PlayerInput input;

	[ValidateInput("StringNotEmpty", "A valid Rewired Action name is needed here")]
	[SerializeField, Tooltip("The Rewired Action name for toggling Measure Mode")]
	private string modeToggleActionName = "Measure Toggle";
	#endregion Variables

	#region MonoBehaviour
	private void OnEnable()
	{
		input.Player.AddInputEventDelegate(ToggleMeasureMode, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, modeToggleActionName);
	}

	private void OnDisable()
	{
		input.Player.RemoveInputEventDelegate(ToggleMeasureMode, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, modeToggleActionName);
	}
	#endregion MonoBehaviour

	#region Private Methods
	/// <summary>
	/// Toggle whether Measure Mode is enabled
	/// </summary>
	/// <param name="_eventData">Rewired input event data</param>
	private void ToggleMeasureMode(InputActionEventData _eventData)
	{
		// Do nothing if game is paused
		if(PauseManager.GetInstance().GetIsPaused())
		{
			return;
		}

		MeasureModeManager.GetInstance().ToggleMeasureMode();
	}
	#endregion Private Methods

	#region Odin Validation
	/// <summary>
	/// Check that a string is not empty (used to validate Rewired input action names are not empty)
	/// </summary>
	/// <param name="_text">The string to check</param>
	/// <returns>True if the inputted string is not null or empty</returns>
	private bool StringNotEmpty(string _text)
	{
		return !string.IsNullOrEmpty(_text);
	}
	#endregion Odin Validation
}