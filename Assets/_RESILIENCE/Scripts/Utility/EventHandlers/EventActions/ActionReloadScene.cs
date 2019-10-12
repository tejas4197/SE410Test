using UnityEngine.SceneManagement;

public class ActionReloadScene : EventAction
{
	#region Protected Methods
	/// <summary>
	/// Toggle the game's paused status
	/// </summary>
	protected override void ExecuteAction()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	#endregion Protected Methods
}