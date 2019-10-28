using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReceptionCenter : Building
{
    public float timeToProcess = 2;
    public Coroutine processing = null;
    private Queue<Refugee> queue = new Queue<Refugee>();
    public Vector3 beforeProcessingPos;
    public Vector3 lineStartPos;
    public Vector3 finishedProcessingPos;

    public float lineOffset = 1.5f;

    public override void BuildingPlaced()
    {
    }
    protected override void customStart()
    {
        StartCoroutine(BuildingConstructionBegin());
    }

    protected override IEnumerator BuildingConstructionBegin()
    {
        yield return new WaitForTicks(constructionTime);

        StartCoroutine(ProcessAllRefugees());
    }

    /// <summary>
    /// Upgrades reception center speed
    /// </summary>
    public override void Upgrade()
    {
        timeToProcess = timeToProcess / 2;
        upgradeLevel++;
    }

    public void AddRefugee(Refugee newRefugee)
    {
        queue.Enqueue(newRefugee);
        newRefugee.SetNewDestination(beforeProcessingPos + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
    }

    private IEnumerator ProcessAllRefugees()
    {
        while (true)
        {
            if (queue.Count == 0)
            {
                yield return new WaitForTicks(1);
            }
            else
            {
                Refugee current = queue.Dequeue();
                current.SetNewDestination(lineStartPos);
                yield return new WaitForTicks(timeToProcess);
                current.SetNewDestination(finishedProcessingPos);
                HousingManager.GetInstance().GetBestHouse().AddOccupant(current);
            }
        }
    }
}
