using UnityEngine;

using Sirenix.OdinInspector;

public class REEIncomeAdd : RandomEventEffect
{
	#region Variables
	[SerializeField, Tooltip("The amount of money to add to the player's funds (or subtract if negative)")]
	int funds = 0;
	#endregion Variables

	#region Protected Methods
	/// <summary>
	/// Add the amount specified by the funds variable to the player's total funds
	/// </summary>
	protected override void CustomRun()
	{
		IncomeManager.GetInstance().AddEventIncome(funds);
	}
	#endregion Protected Methods

	#region Odin Validation
	/// <summary>
	/// Check whether a number is non-zero
	/// </summary>
	/// <param name="_num">The number to check</param>
	/// <returns>True if the number given is non-zero and false otherwise</returns>
	private bool IntNotZero(int _num)
	{
		return _num != 0;
	}
	#endregion Odin Validation
}