using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : Singleton<UIManager>
{
    public Text measureModeIndicator;
    public Text housingCurrentText;
    public Text housingNeedsText;
    public Text refugeesText;
    public Text fundsText;

    public GameObject SkyViewCanvas;
    public GameObject RefugeePanel;

	private void Start()
	{
		MeasureModeManager.GetInstance().OnMeasureModeChange += SetMeasureMode;
	}

	private void OnDisable()
	{
		MeasureModeManager.GetInstance().OnMeasureModeChange -= SetMeasureMode;
	}

    void Start()
    {
        SetMeasureMode(MeasureModeManager.GetInstance().GetMeasureMode());
    }

	public void SetMeasureMode(bool isMeasureMode)
    {
        measureModeIndicator.text = "Measure Mode: " + (isMeasureMode ? "On" : "Off");
        if (isMeasureMode == true)
        {
            SkyViewCanvas.SetActive(true);
            RefugeePanel.SetActive(false);
        }
        else
        {
            SkyViewCanvas.SetActive(false);
            RefugeePanel.SetActive(true);
        }
    }


    public void SetHousingCurrentText(float newHousingCurrent)
    {
        housingCurrentText.text = newHousingCurrent.ToString();
    }

    public void SetHousingNeedText(float newHousingNeeds)
    {
        housingNeedsText.text = newHousingNeeds.ToString();
    }

    public void SetRefugeeText(int newRefugees)
    {
        refugeesText.text = newRefugees.ToString();
    }

    public void SetFundsText(int newFunds)
    {
        fundsText.text = "$" + newFunds.ToString();
    }

    public void AddRefugeesButton()
    {
        RefugeeManager.GetInstance().AddRefugees(20);
    }
}
