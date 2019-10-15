using UnityEngine;

using Sirenix.OdinInspector;

using Rewired;

/// <summary>
/// 2D top-down player movement character controller
/// </summary>
public class PlayerWalk2D : MonoBehaviour
{
	#region Variables
	[Header("Input")]

	[Required]
	[SerializeField, Tooltip("The script for monitoring Rewired player input")]
	private PlayerInput input;

	[Required]
	[SerializeField, Tooltip("The controller for handling movement")]
	private CharacterController controller;

	[ValidateInput("StringNotEmpty", "A valid Rewired Action name is needed here")]
	[SerializeField, Tooltip("The Rewired Action name for horizontal movement")]
	private string horzMoveActionName = "Move Horizontal";

	[ValidateInput("StringNotEmpty", "A valid Rewired Action name is needed here")]
	[SerializeField, Tooltip("The Rewired Action name for vertical movement")]
	private string vertMoveActionName = "Move Vertical";

	[Header("Speed")]

	[MinValue(0f)]
	[SerializeField, Tooltip("The speed at which the player moves")]
	private float walkSpeed = .5f;

	[MinValue(0f)]
	[SerializeField, Tooltip("The speed at which the player turns")]
	private float rotSpeed = 300;

	[Required]
	[SerializeField, Tooltip("The GameObject containing the model for the player body")]
	private GameObject charBody;
	#endregion Variables

	#region MonoBehaviour
	private void Update()
	{
		float moveHorizontal = input.Player.GetAxis(horzMoveActionName);
		float moveVertical = input.Player.GetAxis(vertMoveActionName);

		if(PauseManager.GetInstance().GetIsPaused() || !input.GetCanMove())
		{
			moveHorizontal = 0;
			moveVertical = 0;
		}

		TryMove(moveHorizontal, moveVertical);
	}
	#endregion MonoBehaviour

	#region Private Methods
	/// <summary>
	/// Try moving the player based on horizontal and vertical input values
	/// </summary>
	/// <param name="_horizontal">The value of the horizontal input axis in range [-1, 1]</param>
	/// <param name="_vertical">The value of the vertical input axis in range [-1, 1]</param>
	private void TryMove(float _horizontal, float _vertical)
	{
		controller.Move(new Vector3(_horizontal, 0, _vertical).normalized * walkSpeed);

		if (_horizontal != 0.0f || _vertical != 0.0f)
		{
			Quaternion targetRot = Quaternion.LookRotation(new Vector3(_horizontal, 0, _vertical));
			charBody.transform.rotation = Quaternion.RotateTowards(charBody.transform.rotation, targetRot, rotSpeed * UnityEngine.Time.deltaTime);
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