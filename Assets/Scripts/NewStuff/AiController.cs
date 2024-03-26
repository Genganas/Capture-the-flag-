using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;


public class AIController : MonoBehaviour
{
    public enum AIState { Patrol, Chase, Attack, Capture, Powerup };
    public AIState currentState = AIState.Patrol;

    public float detectionRange = 10f; // Detection range for player
    public float attackRange = 5f; // Attack range for shooting
    public float captureRange = 2f; // Range for capturing flags
    public Transform[] patrolPoints; // Array of patrol points
    public GameObject bulletPrefab; // Prefab of bullet object
    public Transform firePoint; // Point where bullets are fired from
    public float fireRate = 1f; // Rate of fire (bullets per second)
    public LayerMask powerupLayer; // Layer mask for powerup objects
    public float powerupDetectionRange = 5f; // Detection range for powerups
    public float jumpForce = 10f; // Force applied for jumping

    private Transform target; // Target to chase (player or flag or powerup)
    private int currentPatrolIndex = 0;
    private NavMeshAgent agent;
    private float nextFireTime;
    public GameObject player;

    public AudioSource shootSound;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange; // Set stopping distance for agent
        agent.autoBraking = false; // Prevent stopping when reaching destination

        // Start in the initial state
        EnterState(currentState);
    }

    void Update()
    {
        // Update state behavior based on the current state
        switch (currentState)
        {
            case AIState.Patrol:
                PatrolState();
                break;
            case AIState.Chase:
                ChaseState();
                break;
            case AIState.Attack:
                AttackState();
                break;
            case AIState.Capture:
                CaptureState();
                break;
            case AIState.Powerup:
                PowerupState();
                break;
        }

        // Check for nearby powerups
        DetectPowerup();
    }

    // Method to enter a new state
    void EnterState(AIState newState)
    {
        currentState = newState;
        // Perform any state-specific initialization here
    }

    // Method to transition to a new state
    void ChangeState(AIState newState)
    {
        if (currentState != newState)
        {
            ExitState(currentState);
            EnterState(newState);
        }
    }

    // Method to exit the current state
    void ExitState(AIState currentState)
    {
        // Perform any state-specific cleanup here
    }

    void PatrolState()
    {
        if (patrolPoints.Length > 0)
        {
            // Set destination to current patrol point
            agent.destination = patrolPoints[currentPatrolIndex].position;

            // Check if AI has reached the current patrol point
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                // Move to the next patrol point in the list
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                Jump();

                // Check if all patrol points have been visited
                if (currentPatrolIndex == 0)
                {
                    // Transition to Capture State if all points visited
                    ChangeState(AIState.Capture);
                    return;
                }
            }
        }

        if (PlayerInRange())
        {
            
            ChangeState(AIState.Chase);
            return;
        }
    }
    public float jumpDistance = 2f;
    void ChaseState()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
           
            target = player.transform;
            agent.destination = target.position;

            // Check if the player is within jumping distance

            Jump();

            if (PlayerInRange())
            {
                ChangeState(AIState.Attack);
            }
        }
        else
        {
          
            ChangeState(AIState.Patrol);
        }
    }

    // Add method for jumping
    void Jump()
    {
        if (agent.isOnNavMesh) // Ensure the agent is on a NavMesh before jumping
        {
            // Temporarily set destination to current position to stop navigation
            Vector3 currentPos = transform.position;
            agent.destination = currentPos + Vector3.up * 0.1f; // Offset slightly above current position
            agent.velocity += Vector3.up * jumpForce; // Apply jump force

            // After a short delay, reset destination to resume navigation
            StartCoroutine(ResetDestination(currentPos, 0.5f)); // Adjust the delay as needed
        }
    }

    void AttackState()
    {
        if (target != null)
        {
            // Calculate the direction towards the target
            Vector3 direction = (target.transform.position - transform.position).normalized;

            // Calculate the horizontal rotation to face the target
            Quaternion horizontalRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

            // Calculate the vertical angle towards the target
            Vector3 targetDir = target.transform.position - transform.position;
            float verticalAngle = Vector3.Angle(targetDir, transform.forward);
            float verticalRotationValue = Mathf.Clamp(-targetDir.y, -45f, 45f); // Adjust this range as needed
            Quaternion verticalRotation = Quaternion.Euler(verticalRotationValue, 0f, 0f);

            // Combine the horizontal and vertical rotations
            Quaternion lookRotation = horizontalRotation * verticalRotation;

            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Move towards the target
            agent.destination = target.transform.position;

            if (!PlayerInRange())
            {
                // Player is out of range, transition to Patrol state
                ChangeState(AIState.Patrol);
                return;
            }

            // Calculate the angle between the AI and the target's vertical direction
            Vector3 targetVerticalDir = target.transform.position - transform.position;
            verticalAngle = Vector3.Angle(targetVerticalDir, transform.up);

            if (verticalAngle > 90) // If the player is above the AI
            {
                // Shoot up at the player
                firePoint.LookAt(target.transform.position);
            }
            else
            {
                // Shoot horizontally or down at the player
                firePoint.LookAt(target.transform.position);
            }

            if (Time.time >= nextFireTime)
            {
                // Shoot at the target
                Shoot();

                // Update next fire time based on fire rate
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
        else
        {
            ChangeState(AIState.Capture);
        }
    }
    private ScoreManager scoreManager; 
    void CaptureState()
    {
        GameObject flagObject = GameObject.FindGameObjectWithTag("RedFlag"); // Find the flag object by tag
        if (flagObject != null)
        {
            Vector3 flagPosition = flagObject.transform.position; // Get the position of the flag
            agent.destination = flagPosition; // Move directly to the flag position
            agent.stoppingDistance = captureRange; // Set stopping distance to capture range

            // Check if the AI has reached the flag
            if (Vector3.Distance(transform.position, flagPosition) <= captureRange)
            {
                // AI has reached the flag, trigger the OnTriggerEnter method
                flagObject.GetComponent<Flag>().OnTriggerEnter(GetComponent<Collider>());

             

                // Check if the player is in range
                if (PlayerInRange())
                {
                   
                    ChangeState(AIState.Attack);
                }
                else
                {
                    // Set the destination to the red base if player is not in range
                    GameObject redBase = GameObject.FindGameObjectWithTag("RedBase");
                    if (redBase != null)
                    {
                        agent.destination = redBase.transform.position;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Flag not found!");
            // No flag found, continue patrolling
            ChangeState(AIState.Patrol);
        }
    }


    // OnTriggerEnter method to detect when AI reaches the flag or the base
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedFlag"))
        {
            // Flag reached, capture it
            other.transform.SetParent(transform); // Capture the flag by parenting it to the AI
            GameObject baseObject = GameObject.FindGameObjectWithTag("RedBase"); // Find the base object by tag
            if (baseObject != null)
            {
                Vector3 basePosition = baseObject.transform.position; // Get the position of the base
                                                                      // Debug.Log("Moving to base at position: " + basePosition);
                agent.SetDestination(basePosition); // Move towards the base
            }
        }
        else if (other.CompareTag("RedBase"))
        {
            // Base reached, return the flag
            Flag flagScript = transform.GetComponentInChildren<Flag>(); // Get the flag script attached to the child
            if (flagScript != null)
            {
                flagScript.ReturnToBase(); // Call the ReturnToBase method of the flag script
            }
          
            ChangeState(AIState.Patrol); // Return to patrol state
        }


    }

    void PowerupState()
    {
    
    }

    void DetectPowerup()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, powerupDetectionRange, powerupLayer);
        if (colliders.Length > 0)
        {
        
        }
    }

    bool PlayerInRange()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            return distance <= detectionRange; // Check if distance is within detection range
        }
        return false;
    }

    void Shoot()
    {
        // Instantiate bullet prefab and shoot towards the target
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(firePoint.forward * 30f, ForceMode.Impulse);
        shootSound.Play();

    }

    IEnumerator ResetDestination(Vector3 destination, float delay)
    {
        yield return new WaitForSeconds(delay);
        agent.destination = destination;
    }
}