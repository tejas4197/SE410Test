using System.Collections;

using UnityEngine;

using Sirenix.OdinInspector;

/// <summary>
/// Animate the camera when toggling between Sky View and Ground View
/// </summary>
public class PlayerMeasureModeCameraToggle : MonoBehaviour
{
	#region Variables
	[Header("Camera Positions")]
	[SerializeField, Tooltip("The position of the camera in Sky View (in local coordinates)")]
	private Vector3 cameraSkyPosition = new Vector3(0, 30, -7.5f);

	[SerializeField, Tooltip("The position of the camera in Ground View (in local coordinates)")]
	private Vector3 cameraGroundPosition = new Vector3(0f, 1.5f, 0f);

	[SerializeField, Tooltip("The rotation of the camera in Sky View (in world rotation coordinates)")]
	private Vector3 cameraMeasureRotation = new Vector3(60f, 0f, 0f);

	[SerializeField, Tooltip("The rotation of the camera in Ground View (in local rotation coordinates)")]
	private Vector3 cameraGroundRotation = new Vector3(0f, 0f, 0f);

	[Header("Animation Settings")]

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
			StartCoroutine(MoveCamera(cameraSkyPosition, cameraMeasureRotation, _measureMode, cameraMoveDuration));
		}
		else
		{
			StopAllCoroutines();
			StartCoroutine(MoveCamera(cameraGroundPosition, cameraGroundRotation, _measureMode, cameraMoveDuration));
		}
	}

	/// <summary>
	/// Transition the camera from its starting position to a new position
	/// </summary>
	/// <param name="targetPos">The new location to move to</param>
	/// <param name="targetRot">The new rotation to move to</param>
	/// <param name="measureMode">Whether moving into or out of measure mode</param>
	/// <param name="duration">The amount of time the transition will take</param>
	IEnumerator MoveCamera(Vector3 targetPos, Vector3 targetRot, bool measureMode, float duration)
	{
		Vector3 startPos = playerCam.transform.localPosition;
		Vector3 startRot = ((measureMode) ? playerCam.transform.eulerAngles : playerCam.transform.localEulerAngles);

		float deltaX = targetPos.x - startPos.x;
		float deltaY = targetPos.y - startPos.y;
		float deltaZ = targetPos.z - startPos.z;

		float rotDeltaX = targetRot.x - startRot.x;
		float rotDeltaY = targetRot.y - startRot.y;
		float rotDeltaZ = targetRot.z - startRot.z;

		// Check that we're not spinning more than 180 degrees in any direction
		if(rotDeltaX > 180)
		{
			rotDeltaX = -360 + rotDeltaX;
		}
		else if(rotDeltaX < -180)
		{
			rotDeltaX = 360 + rotDeltaX;
		}
		if (rotDeltaY > 180)
		{
			rotDeltaY = -360 + rotDeltaY;
		}
		else if (rotDeltaY < -180)
		{
			rotDeltaY = 360 + rotDeltaY;
		}
		if (rotDeltaZ > 180)
		{
			rotDeltaZ = -360 + rotDeltaZ;
		}
		else if (rotDeltaZ < -180)
		{
			rotDeltaZ = 360 + rotDeltaZ;
		}

		float timer = 0;

		while (timer < duration)
		{
			timer += UnityEngine.Time.deltaTime;
			float progress = timer / duration;

			Vector3 newPos = startPos;
			newPos.x += deltaX * interpolationPath.Evaluate(progress);
			newPos.y += deltaY * interpolationPath.Evaluate(progress);
			newPos.z += deltaZ * interpolationPath.Evaluate(progress);

			playerCam.transform.SetLocalPosition(newPos);
			//playerCam.transform.LookAt(transform.position);

			Vector3 newRot = startRot;
			newRot.x += rotDeltaX * interpolationPath.Evaluate(progress);
			newRot.y += rotDeltaY * interpolationPath.Evaluate(progress);
			newRot.z += rotDeltaZ * interpolationPath.Evaluate(progress);

			if (measureMode)
			{
				playerCam.transform.SetRotation(newRot);
			}
			else
			{
				playerCam.transform.SetLocalRotation(newRot);
			}

			yield return null;
		}
	}
	#endregion Private Methods
}
