using UnityEngine;

/// <summary>
/// Set the mode that controls the Cursor's behavior, such as locking it to the window or the center of the screen
/// </summary>
public class ActionCursorLockModeSet : EventAction
{
	#region Variables
	[SerializeField, Tooltip("The Cursor mode to set")]
	CursorLockMode lockMode;
	#endregion Variables

	#region Protected Methods
	protected override void ExecuteAction()
	{
		Cursor.lockState = lockMode;
	}
	#endregion Protected Methods
}