using UnityEngine;

public abstract class RandomEventEffectExplicitEnd : RandomEventEffect
{
	/// <summary>
	/// Carry out the effect
	/// </summary>
	protected override void ExecuteAction()
	{
		CustomRun();

		// The line below is not commented out in RandomEventEffect but is commented out here because this method must be called explicitly in CustomRun() in child classes
		// InvokeEffectEnd();
	}
}