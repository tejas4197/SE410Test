using System.Collections;

using UnityEngine;

using Sirenix.OdinInspector;

using Rewired;

/// <summary>
/// 3D, first-person player movement controller
/// </summary>
public class PlayerWalk3D : MonoBehaviour
{
	#region Variables
	[Header("Input Settings")]

	[Required]
	[SerializeField, Tooltip("The PlayerInput script for the player")]
	PlayerInput input;

	[ValidateInput("StringNotEmpty", "You must provide a non-empty string")]
	[SerializeField, Tooltip("The Rewired action name for moving forward and backward")]
	string walkActionName = "Walk";

	[ValidateInput("StringNotEmpty", "You must provide a non-empty string")]
	[SerializeField, Tooltip("The Rewired action name for moving left and right")]
	string strafeActionName = "Strafe";

	[ValidateInput("StringNotEmpty", "You must provide a non-empty string")]
	[SerializeField, Tooltip("The Rewired action name for toggling sprint")]
	string sprintActionName = "Sprint";

	[Required]
	[SerializeField, Tooltip("The CharacterController for the player")]
	CharacterController controller;

	[Header("Walking")]

	[MinValue(0f)]
	[SerializeField, Tooltip("The starting speed when moving before accelerating")]
	float startWalkSpeed = 5f;

	[MinValue(0f)]
	[SerializeField, Tooltip("The max walking speed after full acceleration")]
	float maxWalkSpeed = 10f;

	[MinValue(0f)]
	[SerializeField, Tooltip("The acceleration when starting to walk")]
	float walkAcceleration = 0.1f;

	[Header("Sprinting")]

	[MinValue(0f)]
	[SerializeField, Tooltip("The sprinting speed")]
	float sprintSpeed = 13f;

	[MinValue(0f)]
	[SerializeField, Tooltip("The acceleration when starting to sprint")]
	float sprintAcceleration = 0.2f;

	[MinValue(0f)]
	[SerializeField, Tooltip("The max energy the player has for sprinting")]
	float maxSprintEnergy = 100f;

	[MinValue(0f)]
	[SerializeField, Tooltip("The amount of energy the player loses each frame when sprinting")]
	float sprintEnergyDepletion = 1f;

	[ReadOnly]
	[SerializeField, Tooltip("The current amount of energy the player has")]
	float currentEnergy;

	[ReadOnly]
	[SerializeField, Tooltip("The current movement speed")]
	float currentSpeed;

	/// <summary>
	/// Whether the player is walking
	/// </summary>
	bool isWalking = false;

	[ReadOnly]
	[SerializeField, Tooltip("Whether the player is sprinting")]
	bool isSprinting = false;
	#endregion Variables

	#region Getters & Setters
	/// <summary>
	/// Wkhether the player is walking
	/// </summary>
	public bool GetIsWalking()
	{
		return isWalking;
	}

	/// <summary>
	/// Set whether the player is walking
	/// </summary>
	/// <param name="_isWalking">Whether the player is walking</param>
	private void SetIsWalking(bool _isWalking)
	{
		if (isWalking != _isWalking)
		{
			isWalking = _isWalking;

			if (isWalking)
			{
				OnStartMoving?.Invoke();
			}
			else
			{
				SetIsSprinting(false);

				OnStopMoving?.Invoke();
			}
		}
	}

	/// <summary>
	/// Whether the player is sprinting
	/// </summary>
	public bool GetIsSprinting()
	{
		return isSprinting;
	}

	/// <summary>
	/// Set whether the player is sprinting
	/// </summary>
	/// <param name="_isSprinting">Whether the player is sprinting</param>
	private void SetIsSprinting(bool _isSprinting)
	{
		if (isSprinting != _isSprinting)
		{
			isSprinting = _isSprinting;

			if (isSprinting)
			{
				StartCoroutine("Sprint");
				OnStartSprinting?.Invoke();
			}
			else
			{
				StartCoroutine("SprintRecharge");
				OnStopSprinting?.Invoke();
			}
		}
	}
	#endregion Getters & Setters

	#region Events
	/// <summary>
	/// Handler for event invoked when the player starts moving
	/// </summary>
	public delegate void onStartMoving();
	/// <summary>
	/// Event invoked when the player starts moving
	/// </summary>
	public event onStartMoving OnStartMoving;

	/// <summary>
	/// Handler for event invoked when the player stops moving
	/// </summary>
	public delegate void onStopMoving();
	/// <summary>
	/// Event invoked when the player stops moving
	/// </summary>
	public event onStopMoving OnStopMoving;

	/// <summary>
	/// Handler for event invoked when the player starts sprinting
	/// </summary>
	public delegate void onStartSprinting();
	/// <summary>
	/// Event invoked when the player starts sprinting
	/// </summary>
	public event onStartSprinting OnStartSprinting;

	/// <summary>
	/// Handler for event invoked when the player stops sprinting
	/// </summary>
	public delegate void onStopSprinting();
	/// <summary>
	/// Event invoked when the player stops sprinting
	/// </summary>
	public event onStopSprinting OnStopSprinting;
	#endregion Events

	#region MonoBehaviour
	private void OnEnable()
	{
		input.Player.AddInputEventDelegate(TryWalk, UpdateLoopType.Update, InputActionEventType.AxisActive, walkActionName);
		input.Player.AddInputEventDelegate(TryWalk, UpdateLoopType.Update, InputActionEventType.AxisActive, strafeActionName);
		input.Player.AddInputEventDelegate(TrySprint, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, sprintActionName);
		input.Player.AddInputEventDelegate(TrySprint, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, sprintActionName);

		currentSpeed = startWalkSpeed;

		currentEnergy = maxSprintEnergy;
	}

	private void OnDisable()
	{
		input.Player.RemoveInputEventDelegate(TryWalk, UpdateLoopType.Update, InputActionEventType.AxisActive, walkActionName);
		input.Player.RemoveInputEventDelegate(TryWalk, UpdateLoopType.Update, InputActionEventType.AxisActive, strafeActionName);
		input.Player.RemoveInputEventDelegate(TrySprint, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, sprintActionName);
		input.Player.RemoveInputEventDelegate(TrySprint, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, sprintActionName);
	}
	#endregion MonoBehaviour

	#region Private Methods
	/// <summary>
	/// Try walking when receiving Rewired input
	/// </summary>
	/// <param name="_eventData">The Rewired input event data</param>
	private void TryWalk(InputActionEventData _eventData)
	{
		float walkAxis = input.Player.GetAxis(walkActionName);
		float strafeAxis = input.Player.GetAxis(strafeActionName);

		if(PauseManager.GetInstance().GetIsPaused() || !input.GetCanMove())
		{
			walkAxis = 0;
			strafeAxis = 0;
		}

		// This function is called when either axis is active, so this is a way to ensure that, when both axes are active, the function only runs once per frame
		// Arbitrary way to decide which one is the one that exits without doing anything
		if(_eventData.actionName == walkActionName)
		{
			if (walkAxis <= strafeAxis)
			{
				return;
			}
		}
		else
		{
			if (strafeAxis < walkAxis)
			{
				return;
			}
		}

		// Player is not moving
		if (walkAxis == 0 && strafeAxis == 0)
		{
			currentSpeed = startWalkSpeed;
			SetIsWalking(false);
			SetIsSprinting(false);

			// Restore sprint completely if the player stops moving
			currentEnergy = maxSprintEnergy;
		}
		else
		{
			Vector3 moveVector = Vector3.zero;
			Vector2 inputAxes = new Vector2(strafeAxis, walkAxis);

			moveVector += transform.forward * inputAxes.y;
			moveVector += transform.right * inputAxes.x;

			if (moveVector.magnitude > 1)
			{
				moveVector.Normalize();
			}

			moveVector *= currentSpeed;

			controller.SimpleMove(moveVector);

			// ACCELERATION
			if (!isSprinting)
			{
				if (currentSpeed < maxWalkSpeed)
				{
					currentSpeed = Mathf.Clamp(currentSpeed + walkAcceleration, startWalkSpeed, maxWalkSpeed);
				}
				else if (currentSpeed > maxWalkSpeed)
				{
					currentSpeed = Mathf.Clamp(currentSpeed - sprintAcceleration, maxWalkSpeed, sprintSpeed);
				}
			}
			else
			{
				if (currentSpeed < sprintSpeed)
				{
					currentSpeed = Mathf.Clamp(currentSpeed + sprintAcceleration, startWalkSpeed, sprintSpeed);
				}
			}

			SetIsWalking(true);
		}
	}

	/// <summary>
	/// Start or stop sprinting based on player button input
	/// </summary>
	/// <param name="_eventData">The Rewired input event data</param>
	private void TrySprint(InputActionEventData _eventData)
	{
		SetIsSprinting(isWalking && _eventData.GetButton());
	}

	/// <summary>
	/// Deplete the sprint energy as time passes while player sprints
	/// </summary>
	IEnumerator Sprint()
	{
		while (isSprinting)
		{
			yield return new WaitForFixedUpdate();

			currentEnergy = Mathf.Clamp(currentEnergy - sprintEnergyDepletion, 0, maxSprintEnergy);

			if (currentEnergy == 0)
			{
				SetIsSprinting(false);
			}
		}
	}

	/// <summary>
	/// Recharge sprint energy as time passes while player isn't sprinting
	/// </summary>
	IEnumerator SprintRecharge()
	{
		while (!isSprinting)
		{
			yield return new WaitForFixedUpdate();

			// Regain energy if the player is not sprinting but is still moving
			currentEnergy = Mathf.Clamp(currentEnergy + sprintEnergyDepletion, 0, maxSprintEnergy);
		}
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