using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Outline outline;
    
    public float goSpeed = 1;
    public float rotateSpeed = 0;

    private float lastRotationY;
    private void Update()
    {
        agent.speed = (Mathf.Abs(transform.eulerAngles.y - lastRotationY) > 0.5f) ? rotateSpeed : goSpeed;
        
        lastRotationY = transform.rotation.eulerAngles.y;
    }

    public void SetTarget(Vector3 target)
    {
        agent.SetDestination(target);
    }
    
    public void SetOutline(bool value)
    {
        outline.enabled = value;
    }
}
