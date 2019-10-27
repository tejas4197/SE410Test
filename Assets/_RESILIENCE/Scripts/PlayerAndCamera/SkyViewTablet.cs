using UnityEngine;

using Sirenix.OdinInspector;

public class SkyViewTablet : Singleton<SkyViewTablet>
{
	#region Variables
	/// <summary> Instance of Camera attached to player </summary>
	[Required]
	[SerializeField, Tooltip("Instance of Camera attached to player")]
	Camera playerCamera;

	[Required]
	[SerializeField, Tooltip("The Camera used for the tablet display")]
	Camera tabletCamera;

	[SerializeField, Tooltip("The Layers to check for a raycast click on the tablet")]
	LayerMask tabletLayer;
	#endregion Variables

	#region Public Methods
	/// <summary>
	/// Send a raycast from the current mouse position on the tablet to its corresponding location in actual game space
	/// </summary>
	/// <param name="_layers">The LayerMask to apply to the raycast</param>
	/// <returns>The RaycastHit result of the tablet-to-world raycast (hit.collider == null if nothing is found)</returns>
	public RaycastHit GetTabletMouseHit(LayerMask _layers)
	{
		Vector3 mousePos = Input.mousePosition;
		Ray rayTablet = playerCamera.ScreenPointToRay(mousePos);

		RaycastHit tabletHit;

		// First check that the mouse is hovering over the tablet in Sky view
		if (Physics.Raycast(rayTablet, out tabletHit, Mathf.Infinity, tabletLayer))
		{
			// Mouse is over the tablet, so convert first from tablet's texture space to screen space on the sky view camera
			Vector2 localCoord = tabletHit.textureCoord;

			// Create a ray at the camera point
			Ray rayTabletCam = tabletCamera.ScreenPointToRay(new Vector3(localCoord.x * tabletCamera.pixelWidth, localCoord.y * tabletCamera.pixelHeight));

			RaycastHit targetHit;

			// See where the screen point is in world space and check for a hit
			Physics.Raycast(rayTabletCam, out targetHit, Mathf.Infinity, _layers);

			return targetHit;
		}
		// The mouse was not over the tablet
		else
		{
			return new RaycastHit();
		}
	}
	#endregion Public Methods
}