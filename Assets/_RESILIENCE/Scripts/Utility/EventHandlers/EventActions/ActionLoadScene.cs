using UnityEngine;

using Sirenix.OdinInspector;

public class ActionLoadScene : EventAction
{
	#region Variables
	[ValidateInput("StringNotEmpty")]
	[SerializeField, Tooltip("The name of the scene in the build to load")]
	private string sceneToLoad = "";
	#endregion Variables

	#region Protected Methods
	protected override void ExecuteAction()
	{
		AsyncSceneLoad.GetInstance().LoadScene(sceneToLoad);
	}
	#endregion Protected Methods

	#region Odin Validation
	private bool StringNotEmpty(string _text)
	{
		return !string.IsNullOrEmpty(_text);
	}
	#endregion Odin Validation
}