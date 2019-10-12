using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HousePopup : MonoBehaviour
{
    public Text occupancyText;
    public Text prefOccupancyText;
    public Text maxOccupancyText;
    private House house;

    public void SetHouse(House h)
    {
        house = h;
    }

    private void OnEnable()
    {
        if (house != null)
        {
            occupancyText.text = house.GetOccupantCount().ToString();
            prefOccupancyText.text = house.GetPrefOccupancy().ToString();
            maxOccupancyText.text = house.GetMaxOccupancy().ToString();
        }
    }

}

