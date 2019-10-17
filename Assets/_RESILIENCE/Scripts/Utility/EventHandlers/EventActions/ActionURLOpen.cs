using UnityEngine;

using Sirenix.OdinInspector;

public class ActionURLOpen : EventAction
{
	#region Variables
	[ValidateInput("StringNotEmpty", "A URL must be specified")]
	[SerializeField, Tooltip("The URL to open")]
	private string targetURL = "";
	#endregion Variables

	#region Protected Methods
	protected override void ExecuteAction()
	{
		if (StringNotEmpty(targetURL))
		{
			Application.OpenURL(targetURL);
		}
	}
	#endregion Protected Methods

	#region Odin Validation
	private bool StringNotEmpty(string _text)
	{
		return !string.IsNullOrEmpty(_text);
	}
	#endregion Odin Validation
}