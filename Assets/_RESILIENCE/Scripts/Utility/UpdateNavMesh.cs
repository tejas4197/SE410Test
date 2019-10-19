using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpdateNavMesh : MonoBehaviour
{
    public NavMeshSurface meshSurface;

    private void Update()
    {
        BakeAndClear();
    }
    void BakeAndClear()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            meshSurface.RemoveData();
        }
        else if (Input.GetKeyDown(KeyCode.F11))
        {
            meshSurface.BuildNavMesh();
        }
    }
    
}
