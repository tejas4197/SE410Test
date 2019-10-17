using System.Collections;

using UnityEngine;

using Sirenix.OdinInspector;

public class PlayerInput : UserInput
{
	#region Variables
	[Header("Settings")]

	[SerializeField, Tooltip("Whether the character is being controlled by the player or automatically")]
	private bool isManual = true;

	// Whether the player should have automatic controls on a delay
	private bool autoDelayed = false;

	[SerializeField, Tooltip("Whether or not this character can currently move (set to false during dialogue)")]
	private bool canMove = true;
	#endregion Variables

	#region Getters & Setters
	/// <summary>
	/// Whether the character is being controlled by the player or automatically
	/// </summary>
	public bool GetIsManual()
	{
		return isManual;
	}

	/// <summary>
	/// Set whether the character is being controlled automatically or by the player
	/// </summary>
	/// <param name="_isManual">Whether to allow the player to control their character</param>
	public void SetIsManual(bool _isManual)
	{
		isManual = _isManual;
	}

	/// <summary>
	/// Whether or not this character can currently move (set to false during dialogue)
	/// </summary>
	public bool GetCanMove()
	{
		return canMove;
	}

	/// <summary>
	/// Set whether the player can move
	/// </summary>
	/// <param name="_canMove"></param>
	public void SetCanMove(bool _canMove)
	{
		if (canMove != _canMove)
		{
			canMove = _canMove;

			OnCanMoveToggle?.Invoke(canMove);
		}
	}
	#endregion Getters & Setters

	#region Events
	public delegate void CanMoveToggleEventHandler(bool _canMove);
	public event CanMoveToggleEventHandler OnCanMoveToggle;
	#endregion Events

	#region Protected Methods
	// Use this for initialization
	protected override void CustomAwake()
	{
		if (!isManual)
		{
			isManual = true;
			canMove = false;
			autoDelayed = true;
			StartCoroutine("SetManualAfterDelay");
		}
	}
	#endregion Protected Methods

	#region Private Methods
	private IEnumerator SetManualAfterDelay()
	{
		yield return new WaitForTicks(10);

		isManual = false;
		canMove = true;
	}
	#endregion Private Methods
}