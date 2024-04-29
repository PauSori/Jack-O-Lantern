using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieController : MonoBehaviour
{
    public Transform[] patrolPoints; // Puntos de patrulla
    public float patrolSpeed = 2f; // Velocidad de patrulla
    public float chaseSpeed = 5f; // Velocidad de persecución
    public float detectionRange = 10f; // Rango de detección del jugador
    public float detectionAngle = 60f; // Ángulo de visión del jugador (en grados)
    public float attackRange = 3f;
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private Transform player;
    public bool playerDetected = false;

    public GameObject parentObject;

    public Slider slider;

    public Animator animator;
    public GameObject hit;

    private enum Zombie
    {
        Patrolling,
        Chasing,
        Attacking,
    }
    private Zombie currentState;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = Zombie.Patrolling;
        SetDestinationToNextPatrolPoint();

        playerDetected = false;
        hit.SetActive(false);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case Zombie.Patrolling:
                Patrol();
                
                break;
            case Zombie.Chasing:
                ChasePlayer();
                break;
            case Zombie.Attacking:
                AttackPlayer();
                break;
        }
        if (slider.value <= 0)
        {
            Debug.Log("Muerto");
            OnDestroy();
        }

    }

    private void Hit()
    {
        hit.SetActive(true);
    }
    private void NoHit()
    {
        hit.SetActive(false);
    }

    private void Patrol()
    {
        Debug.Log("Patrol");
        agent.speed = patrolSpeed;
        if (agent.remainingDistance < 0.5f)
            SetDestinationToNextPatrolPoint();

        animator.SetBool("walk", true);
        animator.SetBool("run", false);
        // Detectar al jugador dentro del cono de visión
        Vector3 dirToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
        // Detect player
        if (angleToPlayer < detectionAngle / 2f && dirToPlayer.magnitude < detectionRange && 
            Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            
            currentState = Zombie.Chasing;
            playerDetected = true;
        }
        if (slider.value < 1f && Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            currentState = Zombie.Chasing;
            playerDetected = true;
        }
        //if(Vector3.Distance(transform.position, player.position) < detectionRange)
        //{
        //    currentState = Zombie.Chasing;
        //    playerDetected = true;
        //}
    }

    private void ChasePlayer()
    {
        Debug.Log("Chase");
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        animator.SetBool("walk", false);
        animator.SetBool("run", true);

        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            animator.SetBool("run", false);
            currentState = Zombie.Attacking;
        }
        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            
            currentState = Zombie.Patrolling;
            playerDetected = false;
        }
    }
    private void AttackPlayer()
    {
        Debug.Log("Atacando al player");
        //animator.SetBool("run", false);
        animator.SetBool("attack", true);
        // Calcular la dirección hacia el jugador
        Vector3 dirToPlayer = player.transform.position - transform.position;
        
        //dirToPlayer.y += 1.5f;
        // Rotar el objeto completo hacia esa dirección
        transform.rotation = Quaternion.LookRotation(dirToPlayer);

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            animator.SetBool("attack", false);
            currentState = Zombie.Chasing;
        }
    }

    private void SetDestinationToNextPatrolPoint()
    {
        agent.speed = patrolSpeed;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }
    void OnDestroy()
    {
        PuertaController.instance.RemoveGameObject(parentObject);
        Destroy(parentObject);
        Destroy(gameObject);
    }


    void OnDrawGizmosSelected()
    {
        // Dibuja una esfera roja para el rango melee
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, detectionAngle, detectionRange, 0f, 1f);
    }
}





