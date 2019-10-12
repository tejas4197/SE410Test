using UnityEngine;

public abstract class RandomEventEffect : EventAction
{
	#region Events
	/// <summary>
	/// Handler for event called when effect has been completely carried out
	/// </summary>
	/// <param name="_effect">The effect that has been finished and is invoking the event</param>
	public delegate void EventEffectHandler(RandomEventEffect _effect);

	/// <summary>
	/// Event invoked when the effect has completed fully
	/// </summary>
	public event EventEffectHandler OnEffectEnd;
	#endregion Events

	#region Protected Methods
	/// <summary>
	/// Carry out the effect and then invoke the OnEffectEnd event
	/// </summary>
	protected override void ExecuteAction()
	{
		CustomRun();

		InvokeEffectEnd();
	}

	/// <summary>
	/// Things to do that are custom for each random effect (to be overriden in each child class)
	/// </summary>
	protected abstract void CustomRun();

	/// <summary>
	/// Invoke the OnEffectEnd event
	/// </summary>
	protected void InvokeEffectEnd()
	{
		OnEffectEnd?.Invoke(this);
	}
	#endregion Protected Methods
}