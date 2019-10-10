using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

