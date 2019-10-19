using UnityEngine;

using Sirenix.OdinInspector;

public class ActionMonoBehaviourDisable : EventAction
{
	#region Variables
	[Required]
	[SerializeField, Tooltip("The target MonoBehaviour to enable")]
	MonoBehaviour target;
	#endregion Variables

	#region Protected Methods
	/// <summary>
	/// Enable the target when the event is invoked
	/// </summary>
	protected override void ExecuteAction()
	{
		if (target != null)
		{
			target.enabled = false;
		}
	}
	#endregion Protected Methods
}