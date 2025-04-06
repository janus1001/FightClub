using System;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{
    public GameObject target;
    public NavMeshAgent agent;

    public float goSpeed = 1;
    public float rotateSpeed = 0;

    private void Start()
    {
        //agent.
    }


    private float lastRotationY;
    void Update()
    {
        agent.SetDestination(target.transform.position);

        agent.speed = (Mathf.Abs(transform.eulerAngles.y - lastRotationY) > 0.5f) ? rotateSpeed : goSpeed;
        
        lastRotationY = transform.rotation.eulerAngles.y;
    }
}
