using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    private float housingCurrent;

    public ReceptionCenter receptionCenter;
    [HideInInspector] public Player player;

    protected override void CustomAwake()
    {
        DebugConsole.DisplayConsole();
        player = GameObject.FindObjectOfType<Player>();
    }

    public void AddHousing(float newHousing)
    {
        housingCurrent += newHousing;
        UIManager.GetInstance().SetHousingCurrentText(housingCurrent);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; 
            #endif
        }
        if (Input.GetKey(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}

