using System.Collections;

using UnityEngine;

using Sirenix.OdinInspector;

public class PlayerMeasureModeCameraToggle : MonoBehaviour
{
	#region Variables
	private Vector3 cameraMeasurePosition = new Vector3(0, 30, -7.5f);
	private Vector3 cameraSelectPosition = new Vector3(0, 4f, -10);

	[MinValue(0f)]
	[SerializeField, Tooltip("The amount of time for the camera transition to take")]
	public float cameraMoveDuration;

	[Required]
	[SerializeField, Tooltip("The Camera to move")]
	public Camera playerCam;

	[SerializeField, Tooltip("The AnimationCurve to follow")]
	AnimationCurve interpolationPath = AnimationCurve.EaseInOut(0, 0, 1, 1);
	#endregion Variables

	#region MonoBehaviour
	private void OnEnable()
	{
		MeasureModeManager.GetInstance().OnMeasureModeChange += ToggleCameraPos;
	}

	private void OnDisable()
	{
		MeasureModeManager.GetInstance().OnMeasureModeChange -= ToggleCameraPos;
	}
	#endregion MonoBehaviour

	#region Private Methods
	/// <summary>
	/// Toggle camera based on whether Measure Mode is enabled (delegate listening to MeasureModeManager's OnMeasureModeChange event)
	/// </summary>
	/// <param name="_measureMode">Whether in Measure Mode</param>
	private void ToggleCameraPos(bool _measureMode)
	{
		if (_measureMode)
		{
			StopAllCoroutines();
			StartCoroutine(MoveCamera(cameraMeasurePosition, cameraMoveDuration));
		}
		else
		{
			StopAllCoroutines();
			StartCoroutine(MoveCamera(cameraSelectPosition, cameraMoveDuration));
		}
	}

	/// <summary>
	/// Transition the camera from its starting position to a new position
	/// </summary>
	/// <param name="targetPos">The new location to move to</param>
	/// <param name="duration">The amount of time the transition will take</param>
	IEnumerator MoveCamera(Vector3 targetPos, float duration)
	{
		Vector3 startPos = playerCam.transform.localPosition;

		float deltaX = targetPos.x - startPos.x;
		float deltaY = targetPos.y - startPos.y;
		float deltaZ = targetPos.z - startPos.z;

		float timer = 0;

		while (timer < duration)
		{
			timer += UnityEngine.Time.deltaTime;
			float progress = timer / duration;

			Vector3 newPos = startPos;
			newPos.x += deltaX * interpolationPath.Evaluate(progress);
			newPos.y += deltaY * interpolationPath.Evaluate(progress);
			newPos.z += deltaZ * interpolationPath.Evaluate(progress);

			playerCam.transform.localPosition = newPos;
			playerCam.transform.LookAt(transform.position);
			yield return null;
		}
		playerCam.transform.localPosition = Vector3.Lerp(startPos, targetPos, 1.0f);
		playerCam.transform.LookAt(transform.position);
	}
	#endregion Private Methods
}
