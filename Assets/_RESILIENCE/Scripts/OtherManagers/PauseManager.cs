using UnityEngine;

using Sirenix.OdinInspector;


public class PauseManager : Singleton<PauseManager>
{
	#region VARIABLES
		[OnValueChanged("EditorUpdateIsPaused")]
		[SerializeField, Tooltip("Whether the game is currently paused")]
		bool isPaused = false;
		#endregion VARIABLES

	#region GETTERS_SETTERS
		/// <summary>
		/// Get whether the game is currently paused
		/// </summary>
		public bool GetIsPaused()
		{
			return isPaused;
		}

		/// <summary>
		/// Set whether the game is currently paused, invoking OnIsPausedChange if a new value is given
		/// </summary>
		public void SetIsPaused(bool _isPaused)
		{
			if(isPaused != _isPaused)
			{
				isPaused = _isPaused;

				OnIsPausedChange?.Invoke();
			}
		}
		#endregion GETTERS_SETTERS

	#region EVENTS
		/// <summary>
		/// Delegate for subscribing to when isPaused is changed
		/// </summary>
		public delegate void IsPausedChangeHandler();
		/// <summary>
		/// Event invoked whenever the isPaused value is changed
		/// </summary>
		public event IsPausedChangeHandler OnIsPausedChange;
		#endregion EVENTS

	#region PRIVATE_METHODS
		/// <summary>
		/// Editor-specific function called by Odin when the isPaused value is changed in Unity Inspector to invoke the OnIsPausedChange event
		/// </summary>
		private void EditorUpdateIsPaused()
		{
			OnIsPausedChange?.Invoke();
		}
		#endregion PRIVATE_METHODS
}
