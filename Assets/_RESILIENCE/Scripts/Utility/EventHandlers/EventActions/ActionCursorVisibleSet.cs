using UnityEngine;

/// <summary>
/// Set whether the cursor is visible
/// </summary>
public class ActionCursorVisibleSet : EventAction
{
	#region Variables
	[SerializeField, Tooltip("Whether the cursor should be visible")]
	bool isVisible = true;
	#endregion Variables

	#region Protected Methods
	protected override void ExecuteAction()
	{
		Cursor.visible = isVisible;
	}
	#endregion Protected Methods
}