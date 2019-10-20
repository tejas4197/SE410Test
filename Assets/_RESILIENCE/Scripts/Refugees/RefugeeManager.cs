using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefugeeManager : Singleton<RefugeeManager>
{


    #region Attributes
    [SerializeField] private Transform refugeeParent;
    [SerializeField] private GameObject refugeePrefab;
    [SerializeField] private List<RefugeeArrivals> arrivals;

    private Vector3 refugeeGenPos = new Vector3(0, 0, -20);
    private float mPerPersonHousing = 4;
    private float pauseAtStart = 2;
    private int refugees;
    private List<Refugee> refugeeList;
    private float housingNeed;
    private GameManager gameManager;
    #endregion

    #region Getters
    ///<summary>
    ///Accessor for mPerPersonHousing
    ///</summary>
    public float GetMPerPersonHousing()
    {
        return mPerPersonHousing;
    }

    /// <summary>
    /// Accessor for refugees
    /// </summary>
    public int GetRefugeeCount()
    {
        return refugees;
    }

    /// <summary>
    /// Accessor for list of refugee objects
    /// </summary>
    public List<Refugee> GetRefugees()
    {
        return refugeeList;
    }

    public Refugee GetRefugeeAtIndex(int i)
    {
        return refugeeList[i];
    }
    #endregion

    private void Start()
    {
        gameManager = GameManager.GetInstance();
        refugeeGenPos = refugeeParent.transform.position;
        refugeeList = new List<Refugee>();
        StartCoroutine(CreateNewRefugees());

    }

    private IEnumerator CreateNewRefugees()
    {
        yield return new WaitForTicks(pauseAtStart);
        foreach (RefugeeArrivals r in arrivals)
        {
            for (int i = 0; i < r.numRefugees; i++)
            {
                CreateRefugee();
            }
            AddRefugees(r.numRefugees);
            yield return new WaitForTicks(r.timeToNextArrival);
        }
    }

    public void AddRefugees(int newRefugees)
    {
        refugees += newRefugees;
        UIManager.GetInstance().SetRefugeeText(refugees);
        UIManager.GetInstance().SetHousingNeedText(refugees);
        for (int i = 0; i < newRefugees; i++)
        {
            CreateRefugee();
        }
    }

    private void CreateRefugee()
    {
        GameObject refugee = Instantiate(refugeePrefab, refugeeGenPos, Quaternion.identity, refugeeParent);
        gameManager.receptionCenter.AddRefugee(refugee.GetComponent<Refugee>());
        refugeeList.Add(refugee.GetComponent<Refugee>());
    }

    public void SubtractRefugees(int removedRefugees)
    {
        refugees -= removedRefugees;
        UIManager.GetInstance().SetRefugeeText(refugees);
        UIManager.GetInstance().SetHousingNeedText(refugees);
    }


}

[System.Serializable]
public class RefugeeArrivals
{
    public int numRefugees;
    public int timeToNextArrival;
}
