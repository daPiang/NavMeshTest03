using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 destination;

    public float wanderRadius = 10f;
    public float wanderTimer = 10f;

    public float pauseDuration = 2f;
    public float clumpDistance = 5f;
    public float fleeDistance = 5f; // Distance at which agents start running away

    private bool IsPaused = false;
    private float timer;
    private float pauseTimer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        SetNewRandomDestination();
    }

    private void Update()
    {
        Debug.Log(agent.isOnNavMesh);
        Debug.Log(GameManager.aiState);

        if (GameManager.aiState == GameManager.AIState.Default)
        {
            // Behavior for Default state (you can define this behavior)
            pauseTimer += Time.deltaTime; // Update the pause timer

            if (pauseTimer >= pauseDuration)
            {
                IsPaused = false;
                SetNewRandomDestination(); // Clump together behavior
            }
        }

        if (GameManager.aiState == GameManager.AIState.Action1)
        {
            // Behavior for Action1
            pauseTimer += Time.deltaTime; // Update the pause timer

            if (pauseTimer >= pauseDuration)
            {
                IsPaused = false;
                ClumpWithNearbyAgents(); // Clump together behavior
            }
        }

        if (GameManager.aiState == GameManager.AIState.Action2)
        {
            // Behavior for Action2
            pauseTimer += Time.deltaTime; // Update the pause timer

            if (pauseTimer >= pauseDuration)
            {
                IsPaused = false;
                FleeFromNearbyAgents(); // Run away from nearby agents behavior
            }
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

        if (Random.Range(0, 1f) < 0.8f) // 80% chance of stopping
        {
            IsPaused = true;
            pauseTimer = 0;
        }
    }

    private void ClumpWithNearbyAgents()
    {
        // Find all agents within the clumpDistance
        NPCBehavior[] nearbyAgents = FindObjectsOfType<NPCBehavior>();
        Vector3 averagePosition = Vector3.zero;
        int agentCount = 0;

        foreach (NPCBehavior otherAgent in nearbyAgents)
        {
            if (otherAgent != this && Vector3.Distance(transform.position, otherAgent.transform.position) < clumpDistance)
            {
                averagePosition += otherAgent.transform.position;
                agentCount++;
            }
        }

        if (agentCount > 0)
        {
            // Calculate the average position of nearby agents
            averagePosition /= agentCount;

            // Set the agent's destination to move towards the average position
            agent.SetDestination(averagePosition);
        }
    }

    private void FleeFromNearbyAgents()
    {
        // Find all agents within the fleeDistance
        NPCBehavior[] nearbyAgents = FindObjectsOfType<NPCBehavior>();
        foreach (NPCBehavior otherAgent in nearbyAgents)
        {
            if (otherAgent != this && Vector3.Distance(transform.position, otherAgent.transform.position) < fleeDistance)
            {
                // Calculate a destination away from the other agent
                Vector3 fleeDirection = transform.position - otherAgent.transform.position;
                Vector3 fleeDestination = transform.position + fleeDirection.normalized * fleeDistance;

                // Set the agent's destination to flee from the other agent
                agent.SetDestination(fleeDestination);
            }
        }
    }
}
