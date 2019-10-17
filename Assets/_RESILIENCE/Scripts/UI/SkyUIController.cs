using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyUIController : MonoBehaviour
{
    [SerializeField] private SkyUIView skyUIView;
    public GatherIntelInteractionPoint interactionPoint;
    
    //TODO: On button press enable build mode
    void Start()
    {
        //skyUIView = gameObject.GetComponent<SkyUIView>();
        if (skyUIView == null)
            Debug.LogError("SKY UI VIEW NOT FOUND");

    }

    public void UpdateHud(int _currHealth)
    {
        Debug.Log(skyUIView);
        skyUIView.refugeeHealth.text = _currHealth.ToString();
    }
}
