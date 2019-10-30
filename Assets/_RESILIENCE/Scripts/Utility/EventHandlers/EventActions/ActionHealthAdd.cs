using UnityEngine;

using Sirenix.OdinInspector;

/// <summary>
/// Add a certain value to health when the EventAction is invoked
/// </summary>
public class ActionHealthAdd : EventAction
{
	#region Variables
	[Required]
	[SerializeField, Tooltip("The Health script to modify the currStat value of")]
	Health health;

	[SerializeField, Tooltip("The amount to add to the currStat value of health")]
	int healthChange = 0;
	#endregion Variables

	#region Protected Methods
	protected override void ExecuteAction()
	{
		health.AddCurrStat(healthChange);
	}
	#endregion Protected Methods
}