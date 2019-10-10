using UnityEngine;

using Sirenix.OdinInspector;

using Rewired;

public class UserInput : MonoBehaviour
{
	#region Rewired Setup
	Rewired.Player player;

	public Rewired.Player Player
	{
		get { return player; }
		set { player = value; }
	}

	[MinValue(0)]
	[SerializeField, Tooltip("The Rewired player ID of this character")]
	private int playerId = 0;

	public int PlayerId
	{
		get { return playerId; }
		set
		{
			if (value >= -1)
			{
				playerId = value;

				player = ReInput.players.GetPlayer(playerId);
			}
		}
	}
	#endregion Rewired Setup

	#region MonoBehaviour
	// Use this for initialization
	private void Awake()
	{
		player = ReInput.players.GetPlayer(playerId);

		CustomAwake();
	}

	private void OnEnable()
	{
		CustomOnEnable();
	}

	private void OnDisable()
	{
		CustomOnDisable();
	}
	#endregion MonoBehaviour

	#region Public Methods
	/// <summary>
	/// Set the vibration level of a specific motor on each controller for this player
	/// </summary>
	/// <param name="_motorIndex">The index of the motor to vibrate (greater than -1)</param>
	/// <param name="_level">The level of vibration for the motor in range [0, 1]</param>
	public void SetVibration(int _motorIndex, float _level)
	{
		player.SetVibration(_motorIndex, _level);
	}

	/// <summary>
	/// Set vibration for left and right motors of all controllers for this player
	/// </summary>
	/// <param name="_leftMotorLevel">The level of vibration for the left motor in range [0, 1]</param>
	/// <param name="_rightMotorLevel">The level of vibration for the right motor in range [0, 1]</param>
	public void SetVibration(float _leftMotorLevel, float _rightMotorLevel)
	{
		foreach (Controller controller in player.controllers.Controllers)
		{
			if (controller.GetType() != typeof(Joystick))
			{
				continue;
			}

			Joystick j = (Joystick)controller;


			if (!j.supportsVibration)
			{
				continue;
			}

			j.SetVibration(_leftMotorLevel, _rightMotorLevel);
		}
	}

	/// <summary>
	/// Set vibration for all motors of all controllers for this player for a given duration
	/// </summary>
	/// <param name="_motorLevel">The level of the motor for vibration in range [0, 1]</param>
	/// <param name="_duration">The duration of the vibration (0 is infinite)</param>
	public void SetVibrationForDuration(float _motorLevel, float _duration)
	{
		foreach (Controller controller in player.controllers.Controllers)
		{
			if (controller.GetType() != typeof(Joystick))
			{
				continue;
			}

			Joystick j = (Joystick)controller;

			if (!j.supportsVibration)
			{
				continue;
			}

			for (int i = 0; i < j.vibrationMotorCount; ++i)
			{
				j.SetVibration(i, _motorLevel, _duration, false);
			}
		}
	}

	/// <summary>
	/// Set vibration for left and right motors of all controllers for this player for a given duration
	/// </summary>
	/// <param name="_leftMotorLevel">The level of vibration for the left motor in range [0, 1]</param>
	/// <param name="_rightMotorLevel">The level of vibration for the right motor in range [0, 1]</param>
	/// <param name="_leftDuration">The duration of vibration for the left motor in seconds (0 is infinite)</param>
	/// <param name="_rightDuration">The duration of vibration for the right motor in seconds (0 is infinite)</param>
	public void SetVibrationForDuration(float _leftMotorLevel, float _rightMotorLevel, float _leftDuration, float _rightDuration)
	{
		foreach (Controller controller in player.controllers.Controllers)
		{
			if (controller.GetType() != typeof(Joystick))
			{
				continue;
			}

			Joystick j = (Joystick)controller;

			if (!j.supportsVibration)
			{
				continue;
			}

			j.SetVibration(_leftMotorLevel, _rightMotorLevel, _leftDuration, _rightDuration);
		}
	}

	/// <summary>
	/// Stop vibration on all controllers
	/// </summary>
	public void StopVibration()
	{
		player.StopVibration();
	}
	#endregion Public Methods

	#region Protected Methods
	/// <summary>
	/// Function called in Awake() of parent class and to be overriden in child classes instead of overriding Awake() directly
	/// </summary>
	virtual protected void CustomAwake() { }

	/// <summary>
	/// Function called in OnEnable() of parent class and to be overriden in child classes instead of overriding OnEnable() directly
	/// </summary>
	virtual protected void CustomOnEnable() { }

	/// <summary>
	/// Function called in OnDisable() of parent class and to be overriden in child classes instead of overriding OnDisable() directly
	/// </summary>
	virtual protected void CustomOnDisable() { }
	#endregion Protected Methods
}