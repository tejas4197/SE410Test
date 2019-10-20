using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

using Rewired;

/// <summary>
/// Player controller for looking around
/// </summary>
public class PlayerLook : MonoBehaviour
{
	#region Variables
	[Header("Input Settings")]

	[Required]
	[SerializeField, Tooltip("The PlayerInput script for the player")]
	PlayerInput input;

	[ValidateInput("StringNotEmpty", "You must provide a non-empty string")]
	[SerializeField, Tooltip("The Rewired action name for looking up and down")]
	string lookVertActionName = "Look Vertical";

	[ValidateInput("StringNotEmpty", "You must provide a non-empty string")]
	[SerializeField, Tooltip("The Rewired action name for looking left and right")]
	string lookHorzActionName = "Look Horizontal";

	[Header("Looking")]

	[Required]
	[SerializeField, Tooltip("The GameObject to spin when looking left and right")]
	GameObject spinObject;

	[Required]
	[SerializeField, Tooltip("The GameObject to rotate when looking up and down")]
	GameObject lookObject;

	[MinValue(0f)]
	[SerializeField, Tooltip("The speed when looking horizontally")]
	float lookHorizSpeed = 1f;

	[MinValue(0f)]
	[SerializeField, Tooltip("The speed when looking vertically")]
	float lookVertSpeed = 0.5f;

	[Header("Camera Locking Rotation")]
	[MinValue(-360f), MaxValue(0f)]
	[SerializeField, Tooltip("The minimum rotation allowed for the camera's vertical axis")]
	float minCamHorizRotation = -90f;

	[MinValue(-0f), MaxValue(360f)]
	[SerializeField, Tooltip("The maximum rotation allowed for the camera's vertical axis")]
	float maxCamHorizRotation = 90f;

	[ReadOnly]
	[SerializeField, Tooltip("A list of all scripts 'blocking' the player from looking around")]
	List<MonoBehaviour> lookBlocks = new List<MonoBehaviour>();
	#endregion Variables

	#region MonoBehaviour
	private void OnEnable()
	{
		input.Player.AddInputEventDelegate(TryLook, UpdateLoopType.Update, InputActionEventType.AxisActive, lookVertActionName);
		input.Player.AddInputEventDelegate(TryLook, UpdateLoopType.Update, InputActionEventType.AxisActive, lookHorzActionName);
	}

	private void OnDisable()
	{
		input.Player.RemoveInputEventDelegate(TryLook, UpdateLoopType.Update, InputActionEventType.AxisActive, lookVertActionName);
		input.Player.RemoveInputEventDelegate(TryLook, UpdateLoopType.Update, InputActionEventType.AxisActive, lookHorzActionName);
	}
	#endregion MonoBehaviour

	#region Public Methods
	/// <summary>
	/// Add a block, preventing the player from looking around (when called, just pass in the script that is calling this method with the 'this' keyword)
	/// </summary>
	/// <param name="_block">The script blocking the player from looking around</param>
	public void AddLookBlock(MonoBehaviour _block)
	{
		if(!lookBlocks.Contains(_block))
		{
			lookBlocks.Add(_block);
		}
	}

	/// <summary>
	/// Remove the block, allowing the player to look around again if the list is now empty (when called, just pass in the script that is calling this method with the 'this' keyword)
	/// </summary>
	/// <param name="_block">The block to remove</param>
	public void RemoveLookBlock(MonoBehaviour _block)
	{
		if(lookBlocks.Contains(_block))
		{
			lookBlocks.Remove(_block);
		}
	}
	#endregion Public Methods

	#region Private Methods
	/// <summary>
	/// Try looking when receiving input
	/// </summary>
	/// <param name="_eventData">The Rewired input event data</param>
	private void TryLook(InputActionEventData _eventData)
	{
		/* Player cannot look around if any of the following are true:
		 *	- The game is paused
		 *	- The input script is unable to move
		 *	- Sky View is enabled
		 *	- There is at least one blocker registered on this script
		 */
		if (PauseManager.GetInstance().GetIsPaused() || !input.GetCanMove() || MeasureModeManager.GetInstance().GetMeasureMode() || lookBlocks.Count > 0)
		{
			return;
		}

		float vertAxis = input.Player.GetAxis(lookVertActionName);
		float horzAxis = input.Player.GetAxis(lookHorzActionName);

		spinObject.transform.Rotate(new Vector3(0, lookHorizSpeed * horzAxis, 0));

		ApplyVertRotation(lookVertSpeed * vertAxis);
	}

	/// <summary>
	/// Rotate the player up/down based on look speed
	/// </summary>
	/// <param name="_lookVertSpeed">The speed to move at</param>
	private void ApplyVertRotation(float _lookVertSpeed)
	{
		Vector3 camRotation = new Vector3(lookObject.transform.localEulerAngles.x, 0, 0);

		camRotation.x %= 360;

		if (camRotation.x > 180)
		{
			camRotation.x -= 360;
		}

		camRotation.x -= _lookVertSpeed;

		// Restrict the player to a certain range
		if (camRotation.x < minCamHorizRotation)
		{
			//Debug.Log($"[PlayerLook] Cam rotation too low at {camRotation.x}. Clamping to {minCamHorizRotation}");
			camRotation.x = minCamHorizRotation;
		}
		else if (camRotation.x > maxCamHorizRotation)
		{
			//Debug.Log($"[PlayerLook] Cam rotation too large at {camRotation.x}. Clamping to {maxCamHorizRotation}");
			camRotation.x = maxCamHorizRotation;
		}
		lookObject.transform.localEulerAngles = camRotation;
	}
	#endregion Private Methods

	#region Odin Validation
	/// <summary>
	/// Check whether a string is empty for Odin validation
	/// </summary>
	/// <param name="_text">The string to check</param>
	/// <returns>True if the string is not null or blank and false otherwise</returns>
	private bool StringNotEmpty(string _text)
	{
		return !string.IsNullOrEmpty(_text);
	}
	#endregion Odin Validation
}