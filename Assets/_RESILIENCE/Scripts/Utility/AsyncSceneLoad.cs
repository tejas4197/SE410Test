using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneLoad : Singleton<AsyncSceneLoad>
{
	#region Variables
	// The name of the Main Menu scene
	static readonly string MAIN_MENU_NAME = "MainMenuTest";

	// The amount of percentage that must be loaded to display the scene as done loading
	private readonly float SCENE_LOAD_DONE_PERCENTAGE = 0.9f;

	// The format of methods to call when finished loading a scene
	public delegate void onSceneFinishLoading(AsyncOperation _operation = null);

	private Stack<string> scenesLoaded = new Stack<string>();
	#endregion Variables

	#region Public Methods
	/// <summary>
	/// Go back to the previous scene loaded
	/// </summary>
	/// <param name="onSceneFinishedLoading">The delegate to call when complete</param>
	public void BackScene(onSceneFinishLoading onSceneFinishedLoading = null)
    {
        string sceneToLoad = MAIN_MENU_NAME;

        if(scenesLoaded.Count != 0)
        {
            // Get the next scene to go to
            sceneToLoad = scenesLoaded.Pop();
        }

        Debug.Log($"[AsyncSceneLoad] Going back to scene {sceneToLoad}");

        StartCoroutine(LoadSceneAsync(sceneToLoad, false, onSceneFinishedLoading));
    }

    /// <summary>
    /// Go back to a specified scene previously loaded if at all
    /// </summary>
    /// <param name="sceneToLoad"></param>
    /// <param name="onSceneFinishedLoading"></param>
    public void BackToScene(string sceneToLoad, onSceneFinishLoading onSceneFinishedLoading = null)
    {
        string sceneLoaded = "";

        // Pop scenes off the stack until the scene to return to has been reached or the stack has been emptied
        while(scenesLoaded.Count > 0 && sceneLoaded != sceneToLoad)
        {
            sceneLoaded = scenesLoaded.Pop();
        }

        // Load the scene
        StartCoroutine(LoadSceneAsync(sceneToLoad, false, onSceneFinishedLoading));
    }

	/// <summary>
	/// Load a scene and close the current one
	/// </summary>
	/// <param name="sceneToLoad">The scene to load</param>
	/// <param name="onSceneFinishedLoading">A function to call when finished loading the scene</param>
	public void LoadScene(string sceneToLoad, onSceneFinishLoading onSceneFinishedLoading = null)
	{
        if(sceneToLoad != MAIN_MENU_NAME)
        {
            scenesLoaded.Push(SceneManager.GetActiveScene().name);
        }
        else
        {
            scenesLoaded.Clear();
            scenesLoaded.Push(sceneToLoad);
        }

        StartCoroutine(LoadSceneAsync(sceneToLoad, false, onSceneFinishedLoading));
    }

	/// <summary>
	/// Load a scene using the additive mode (do not close the current scenes when loading a new one)
	/// </summary>
	/// <param name="sceneToLoad">The scene to load</param>
	/// <param name="onSceneFinishedLoading">A function to call when finished loading the scene</param>
	public void LoadSceneAdditive(string sceneToLoad, onSceneFinishLoading onSceneFinishedLoading = null)
	{
		StartCoroutine(LoadSceneAsync(sceneToLoad, true, onSceneFinishedLoading));
	}
	#endregion Public Methods

	#region Private Methods
	/// <summary>
	/// Begin loading the scene and wait until the scene has loaded and tagging animation has finished before switching scenes
	/// </summary>
	/// <param name="sceneToLoad">The name of the scene to load</param>
	/// <param name="loadSceneAdditive">Whether to use the additive scene loading mode</param>
	/// <param name="onSceneFinishedLoading">A function to call on completion of scene loading</param>
	private IEnumerator LoadSceneAsync(string sceneToLoad, bool loadSceneAdditive = false, onSceneFinishLoading onSceneFinishedLoading = null)
	{
		Debug.Log("[AsyncSceneLoad] Scene loading function successfully called");
		// Instantiate sceneLoad here for access later
		AsyncOperation sceneLoad = null;

		bool validSceneName = true;

		// In case the scene name is invalid, use a try-catch block here
		try
		{
			LoadSceneMode sceneLoadingMode = LoadSceneMode.Single;

			if (loadSceneAdditive)
			{
				sceneLoadingMode = LoadSceneMode.Additive;
			}

			sceneLoad = SceneManager.LoadSceneAsync(sceneToLoad.Trim(), sceneLoadingMode);
			Debug.Log("[AsyncSceneLoad] Started loading scene: " + sceneToLoad);
		}
		catch (System.Exception e)
		{
			// The scene name was not valid, so report the name and set validSceneName to false
			Debug.LogError("[AsyncSceneLoad] Could not load the scene with name " + sceneToLoad + ". Check that the scene is included in the build index.");

			validSceneName = false;
		}

		// If the scene name was valid, then move to the next part of scene loading
		if (validSceneName)
		{
			Debug.Log("[AsyncSceneLoad] Scene valid. Scene: " + sceneToLoad);
			// Disable automatic loading of the scene
			sceneLoad.allowSceneActivation = false;

			// Wait until the scene is finished loading
			while (sceneLoad.progress < SCENE_LOAD_DONE_PERCENTAGE)
			{
				yield return null;
			}

			if (onSceneFinishedLoading != null)
			{
				Debug.Log("[AsyncSceneLoad] Finished loading scene and now calling delegate");
				onSceneFinishedLoading(sceneLoad);
			}
			else
			{
				Debug.Log("[AsyncSceneLoad] Finished loading scene and now switching scenes");
				// Scene has loaded, so let the scene switch
				sceneLoad.allowSceneActivation = true;
			}
		}
	}
	#endregion Private Methods
}