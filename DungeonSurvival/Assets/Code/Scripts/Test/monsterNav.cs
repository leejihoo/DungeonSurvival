using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Extensions;
using UnityEngine;

public class monsterNav : MonoBehaviour
{
    public AgentOverride2d temp;

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        temp.Agent.SetDestination(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        temp.Agent.SetDestination(target.position);
    }
}
