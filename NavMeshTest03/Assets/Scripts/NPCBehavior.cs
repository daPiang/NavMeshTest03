using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 destination;

    public float wanderRadius = 10f;
    public float wanderTimer = 10f;

    public float pauseDuration = 2f;

    private bool IsPaused = false;
    private float timer;
    private float pauseTimer;
    // Vector3[]/ or List<Vector3> points

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        SetNewRandomDestination();
    }

    private void Update()
    {
        if(GameManager.aiState == GameManager.AIState.Action1)
        {
            if (pauseTimer >= pauseDuration)
            {
                IsPaused = false;
                SetNewRandomDestination();
            }
        }

        if(GameManager.aiState == GameManager.AIState.Action2)
        {
            //Insert Action2
        }
    }
    private void SetNewRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);

        destination = navHit.position;
        agent.SetDestination(destination);
        timer = 0;

        if (Random.Range(0, 1f) < 0.8f) // 80% chance of stoppping
        {
            IsPaused = true;
            pauseTimer = 0;
        }
    }
}
