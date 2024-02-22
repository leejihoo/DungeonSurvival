using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshTest : MonoBehaviour
{
    public NavMeshSurface temp;
    // Start is called before the first frame update
    void Start()
    {
        temp.BuildNavMeshAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
