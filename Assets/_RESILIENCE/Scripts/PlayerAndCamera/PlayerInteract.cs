using UnityEngine;

using Sirenix.OdinInspector;

using Rewired;

/// <summary>
/// Player controller for interacting with objects in Sky View
/// </summary>
public class PlayerInteract : MonoBehaviour
{
	#region Variables
	[Header("Input")]

	[Required]
	[SerializeField, Tooltip("The script for monitoring Rewired player input")]
	private PlayerInput input;

	[ValidateInput("StringNotEmpty", "A valid Rewired Action name is needed here")]
	[SerializeField, Tooltip("The Rewired Action name for interaction")]
	private string interactActionName = "Interact";


	[Header("Settings")]

	[Required]
	[SerializeField, Tooltip("The Camera used for viewing the game from")]
	private Camera camera;

	/// <summary>
	/// The currently selected object
	/// </summary>
	private Building selected = null;
	#endregion Variables

	#region Getters & Setters
	private void SetSelected(Building _selected)
	{
		if(selected == _selected)
		{
			return;
		}

		// If selected isn't null, then deselect it
		selected?.OnDeselected();

		selected = _selected;

		// TODO: Add in a call to OnSelect again once reimplemented on buildings
		// This was commented out when transitioning from Interactable to Building because Building does not have a method to call when it is selected, only deselected
		// If selected now isn't null, then select it
		//selected?.OnSelected();
	}
	#endregion Getters & Setters

	#region MonoBehaviour
	private void OnEnable()
	{
		input.Player.AddInputEventDelegate(TryInteract, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, interactActionName);

		MeasureModeManager.GetInstance().OnMeasureModeChange += DeselectOnMeasureMode;
	}

	private void OnDisable()
	{
		input.Player.RemoveInputEventDelegate(TryInteract, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, interactActionName);

		MeasureModeManager.GetInstance().OnMeasureModeChange -= DeselectOnMeasureMode;
	}

	private void Update()
	{
		if (!PauseManager.GetInstance().GetIsPaused() && MeasureModeManager.GetInstance().GetMeasureMode())
		{
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

			Physics.Raycast(ray, out hit);

			SetSelected(hit.collider?.gameObject?.GetComponent<Building>());
		}
	}
	#endregion MonoBehaviour

	#region Private Methods
	/// <summary>
	/// Try to interact with the currently selected target after receiving Rewired input event
	/// </summary>
	/// <param name="_eventData">The Rewired input event data</param>
	private void TryInteract(InputActionEventData _eventData)
	{
		// Only interact if not in Measure Mode
		if (MeasureModeManager.GetInstance().GetMeasureMode())
		{
			selected?.OnInteract();
		}
	}
	
	/// <summary>
	/// Deselect whatever's currently selected when toggling Measure Mode
	/// </summary>
	/// <param name="_measureMode">Whether Measure Mode is enabled</param>
	private void DeselectOnMeasureMode(bool _measureMode)
	{
		if(!_measureMode)
		{
			SetSelected(null);
		}
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