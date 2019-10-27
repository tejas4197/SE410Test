using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HousePopup : MonoBehaviour
{
    public Text occupancyText;
    public Text prefOccupancyText;
    public Text maxOccupancyText;
    public Button upgradeButton;
    private House house;

    public void SetHouse(House h)
    {
        house = h;
    }

    private void OnEnable()
    {
        if (house != null)
        {
            TimeManager.GetInstance().OnTick += ToggleButtonEnabled;
            occupancyText.text = house.GetOccupantCount().ToString();
            prefOccupancyText.text = house.GetPrefOccupancy().ToString();
            maxOccupancyText.text = house.GetMaxOccupancy().ToString();
            Button ub = upgradeButton.GetComponent<Button>();
            ub.GetComponentInChildren<Text>().text = "Upgrade " + house.GetUpgradeLevel()+1.ToString() + "- $" + house.GetUpgradePrice().ToString();
            ub.onClick.AddListener(Upgrade);
        }
    }

    private void OnDisable()
    {
        TimeManager.GetInstance().OnTick -= ToggleButtonEnabled;
    }

    private void ToggleButtonEnabled()
    {
        if (IncomeManager.GetInstance().GetFunds() >= house.GetUpgradePrice() && house.GetUpgradeLevel() < 1)
        {
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeButton.interactable = false;
        }
    }

    private void Upgrade()
    {
        if (IncomeManager.GetInstance().GetFunds() >= house.GetUpgradePrice())
        {
            IncomeManager.GetInstance().SpendFunds(house.GetUpgradePrice());
            house.Upgrade();

            // For demo purposes, rework for multi-upgrade
            upgradeButton.GetComponentInChildren<Text>().text = "Upgrade Complete";
            upgradeButton.interactable = false;
        }
    }
   

}

