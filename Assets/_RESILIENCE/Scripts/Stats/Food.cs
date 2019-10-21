using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Int_Stat
{
    private bool isFull = true;
    private bool isStarving = false;

    /// <summary>
    /// Sets whether or not the stat is fully hydrated, dehydrated, or is in between.
    /// </summary>
    protected override void CurrStatChanged()
    {
        if (currStatVal <= minStatVal)
        {
            isStarving = true;
        }
        else if (currStatVal >= maxStatVal)
        {
            isFull = true;
            isStarving = false;
        }
        else
        {
            isFull = false;
            isStarving = false;
        }
    }
}
