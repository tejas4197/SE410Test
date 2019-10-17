using UnityEngine;

public class ActionQuitGame : EventAction
{
	#region Protected Methods
	/// <summary>
	/// Close the game when the button is clicked
	/// </summary>
	protected override void ExecuteAction()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
	}
	#endregion Protected Methods
}