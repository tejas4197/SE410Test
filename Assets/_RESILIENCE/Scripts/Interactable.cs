using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: KILL THIS SCRIPT WITH FIRE
// (Likely will want to have something to replace this, just not this script)
public abstract class Interactable : MonoBehaviour
{
    public Material deselected;
    public Material selected;
    public GameObject indicatorObj;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetMaterial(deselected);
    }

    public void OnSelected()
    {
        SetMaterial(selected);
    }

    public virtual void OnDeselected()
    {
        SetMaterial(deselected);
    }

    private void SetMaterial(Material mat)
    {
        Material[] mats = indicatorObj.GetComponent<Renderer>().materials;
        mats[0] = mat;
        indicatorObj.GetComponent<Renderer>().materials = mats;
    }

    public abstract void OnInteract();
}

