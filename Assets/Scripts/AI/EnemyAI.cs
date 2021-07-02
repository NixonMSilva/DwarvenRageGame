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
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attack
    public float timeBetweenAttacks;
    protected bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Movement
    [SerializeField] protected float rotationSpeed = 10f;
    private Vector3 playerPoint;

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
            //agent.isStopped = false;
            //agent.speed = baseSpeed;

            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange)
            {
                Patroling();
            }
            else if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
            else if (playerInSightRange && playerInAttackRange && !alreadyAttacked)
            {
                AttackPlayer();
            }

            anim.SetFloat("speed", agent.speed);
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

    private void Patroling()
    {
        agent.speed = baseSpeed;

        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        float distanceToWalkPoint = Vector3.Distance(transform.position, walkPoint);

        if (distanceToWalkPoint < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }

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
