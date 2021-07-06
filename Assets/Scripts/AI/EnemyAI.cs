using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    protected Animator anim;

    [SerializeField] private float baseSpeed = 12f; 

    protected AttackController attack;

    private EnemyStatus status;

    public bool isAttacking = false;

    [SerializeField] private Transform feetPosition;

    private int attackDirection = 1;

    // Patrol
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private float walkPointRange;
    [SerializeField] private bool isPatrolling = false;

    // Attack
    public float timeBetweenAttacks;
    protected bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Movement
    [SerializeField] protected float rotationSpeed = 10f;
    private float currSpeed;
    protected Vector3 playerPoint;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponentInChildren<NavMeshAgent>();

        anim = GetComponentInChildren<Animator>();
        attack = GetComponent<AttackController>();
        status = GetComponent<EnemyStatus>();
    }

    private void Update()
    {
        // If the enemy is not dying, the perform AI routines
        if (!status.IsDying && !isAttacking)
        {
            // Update player speed in order to
            // decide which animator status
            // to play
            UpdateSpeed();

            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange)
            {
                Patroling();
            }
            else if (playerInSightRange && !playerInAttackRange)
            {
                isPatrolling = false;
                agent.ResetPath();
                ChasePlayer();
            }
            else if (playerInSightRange && playerInAttackRange && !alreadyAttacked)
            {
                isPatrolling = false;
                agent.ResetPath();
                AttackPlayer();
            }
        }
        else
        {
            agent.SetDestination(transform.position);
        }

        if (isAttacking)
        {
            LookAtPlayer();
        }
    }

    private void UpdateSpeed ()
    {
        currSpeed = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("speed", currSpeed);
    }

    private void Patroling()
    {
        if (!playerInSightRange)
        {
            agent.speed = baseSpeed;

            if (!isPatrolling)
            {
                walkPoint = SearchWalkPoint();
                agent.SetDestination(walkPoint);
                isPatrolling = true;
            }
            else
            {
                if (Vector3.Distance(transform.position, walkPoint) <= 1f)
                    isPatrolling = false;
            }
        }
    }

    private Vector3 SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 newWalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMeshHit hit;

        Debug.DrawRay(newWalkPoint, Vector3.up * 100f, Color.magenta, 2f);

        // If the new random point is inside NavMesh, then move
        if (NavMesh.SamplePosition(newWalkPoint, out hit, 5f, NavMesh.AllAreas))
        {
            return newWalkPoint;
        }

        // Else, stand still and try again
        return transform.position;
    }

    private void ChasePlayer()
    {
        agent.speed = baseSpeed * 2;
        agent.SetDestination(player.position);
    }

    public virtual void AttackPlayer()
    {
        //agent.SetDestination(transform.position);

        playerPoint = player.transform.position;
        //LookAtPlayer();
        StopForAttack();

        if (!alreadyAttacked)
        {

            if (attackDirection == 1)
            {
                anim.Play("attack_right");
            }
            else
            {
                anim.Play("attack_left");
            }

            isAttacking = true;

            attackDirection++;

            if (attackDirection > 2)
            {
                attackDirection = 1;
            }
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    protected void StopForAttack ()
    {
        //agent.speed = 0f;
        //agent.isStopped = true;
        agent.SetDestination(transform.position);
    }

    public void StopForStagger ()
    {
        agent.SetDestination(transform.position);
    }

    protected void LookAtPlayer ()
    {
        Vector3 lookPosition = player.transform.position - transform.position;
        lookPosition.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPosition);
        /*
        Quaternion targetRotation = Quaternion.LookRotation(playerPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); */
        //transform.LookAt(lookPosition);
    }

    protected void ResetAttack()
    {
        alreadyAttacked = false;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
