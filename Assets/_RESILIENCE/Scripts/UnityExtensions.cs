using UnityEngine;

/// <summary>
/// Class containing extensions that can be called by different MonoBehaviour-type objects
/// </summary>
public static class UnityExtensions
{
	/*
	 * All methods below have an implicit parameter as their first parameter
	 * This is the parameter with the keyword "this" before its type
	 * To call the extension function, you must invoke it with an object of the implicit type
	 * For example, transform.SetPosition(targetPos)
	 */

	/// <summary>
	/// Set the position of a Transform
	/// </summary>
	/// <param name="_transform">The Transform to move</param>
	/// <param name="_position">The position to move to</param>
	public static void SetPosition(this Transform _transform, Vector3 _position)
	{
		_transform.position = _position;
	}

	/// <summary>
	/// Set the local position of a Transform
	/// </summary>
	/// <param name="_transform">The Transform to move</param>
	/// <param name="_position">The position to move to</param>
	public static void SetLocalPosition(this Transform _transform, Vector3 _position)
	{
		_transform.localPosition = _position;
	}

	/// <summary>
	/// Move a Transform a certain amount on each axis
	/// </summary>
	/// <param name="_transform">The Transform to move</param>
	/// <param name="_deltaX">The amount to move on the x axis</param>
	/// <param name="_deltaY">The amount to move on the y axis</param>
	/// <param name="_deltaZ">The amount to move on the z axis</param>
	public static void MovePosition(this Transform _transform, float _deltaX, float _deltaY, float _deltaZ)
	{
		_transform.position += new Vector3(_deltaX, _deltaY, _deltaZ);
	}

	/// <summary>
	/// Move a Transform a certain amount on each axis in local space
	/// </summary>
	/// <param name="_transform">The Transform to move</param>
	/// <param name="_deltaX">The amount to move on the x axis</param>
	/// <param name="_deltaY">The amount to move on the y axis</param>
	/// <param name="_deltaZ">The amount to move on the z axis</param>
	public static void MoveLocalPosition(this Transform _transform, float _deltaX, float _deltaY, float _deltaZ)
	{
		_transform.localPosition += new Vector3(_deltaX, _deltaY, _deltaZ);
	}

	/// <summary>
	/// Set the rotation of a Transform with euler angles
	/// </summary>
	/// <param name="_transform">The Transform to rotate</param>
	/// <param name="_eulerAngles">The euler angle rotation to set</param>
	public static void SetRotation(this Transform _transform, Vector3 _eulerAngles)
	{
		_transform.eulerAngles = _eulerAngles;
	}

	/// <summary>
	/// Set the local rotation of a Transform with euler angles
	/// </summary>
	/// <param name="_transform">The Transform to rotate</param>
	/// <param name="_eulerAngles">The euler angle rotation to set</param>
	public static void SetLocalRotation(this Transform _transform, Vector3 _eulerAngles)
	{
		_transform.localEulerAngles = _eulerAngles;
	}

	/// <summary>
	/// Move a Transform's rotation by a certain amount on each axis
	/// </summary>
	/// <param name="_transform">The Transform to rotate</param>
	/// <param name="_deltaX">The amount to rotate on the x axis</param>
	/// <param name="_deltaY">The amount to rotate on the y axis</param>
	/// <param name="_deltaZ">The amount to rotate on the z axis</param>
	public static void MoveRotation(this Transform _transform, float _deltaX, float _deltaY, float _deltaZ)
	{
		_transform.Rotate(new Vector3(_deltaX, _deltaY, _deltaZ), Space.World);
	}

	/// <summary>
	/// Move a Transform's local rotation by a certain amount on each axis
	/// </summary>
	/// <param name="_transform">The Transform to rotate</param>
	/// <param name="_deltaX">The amount to rotate on the x axis</param>
	/// <param name="_deltaY">The amount to rotate on the y axis</param>
	/// <param name="_deltaZ">The amount to rotate on the z axis</param>
	public static void MoveLocalRotation(this Transform _transform, float _deltaX, float _deltaY, float _deltaZ)
	{
		_transform.Rotate(new Vector3(_deltaX, _deltaY, _deltaZ), Space.Self);
	}
}