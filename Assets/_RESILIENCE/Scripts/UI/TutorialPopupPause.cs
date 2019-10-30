using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopupPause : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TimeManager.GetInstance().SetTimePassageMultiplier(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey) { // not using rewired because this is too simple to need it
            TimeManager.GetInstance().SetTimePassageMultiplier(1);
            Destroy(this.gameObject);
        }
    }
}
