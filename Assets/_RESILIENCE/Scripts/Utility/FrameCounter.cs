using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameCounter : MonoBehaviour
{
    private int frameRate;
    public Text framesCounterText;

    public void LateUpdate()
    {
        GetFrames();
    }

    private void GetFrames()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            if (framesCounterText.gameObject.activeSelf == false)
            {
                framesCounterText.gameObject.SetActive(true);
                float current = 0;
                current = Time.frameCount / Time.time;
                frameRate = Mathf.RoundToInt(current);
                framesCounterText.text = frameRate.ToString() + " FPS";
            }
            else if(framesCounterText.gameObject.activeSelf == true)
            {
                framesCounterText.gameObject.SetActive(false);
            }
        }
    }
}
