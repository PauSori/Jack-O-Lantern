using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CalabazaController : MonoBehaviour
{

    public Transform[] patrolPoints; // Puntos de patrulla
    public float patrolSpeed = 2f; // Velocidad de patrulla
    public float chaseSpeed = 5f; // Velocidad de persecución
    public float detectionRange = 10f; // Rango de detección del jugador
    public float detectionAngle = 60f; // Ángulo de visión del jugador (en grados)
    public GameObject bulletPrefab; // Prefab de la bala
    public Transform firePoint; // Punto de disparo
    public float fireRate = 1f;
    public float attackRange = 3f;
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private Transform player;
    public bool playerDetected = false;
    private float nextFireTime = 5f;

    public bool turretMode = false;

    private enum Calabaza
    {
        Patrolling,
        Chasing,
        Attacking,
        Shooting
    }
    private Calabaza currentState;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = Calabaza.Patrolling;
        SetDestinationToNextPatrolPoint();
    }

    private void Update()
    {
        switch (currentState)
        {
            case Calabaza.Patrolling:
                Patrol();
                break;
            case Calabaza.Chasing:
                ChasePlayer();
                break;
            case Calabaza.Attacking:
                AttackPlayer();
                break;
            case Calabaza.Shooting:
                Shooting();
                break;
        }


    }

    private void Patrol()
    {
        Debug.Log("Patrol");
        agent.speed = patrolSpeed;
        if (agent.remainingDistance < 0.5f)
            SetDestinationToNextPatrolPoint();
        
        
        // Detectar al jugador dentro del cono de visión
        Vector3 dirToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
        // Detect player
        if (angleToPlayer < detectionAngle / 2f && dirToPlayer.magnitude < detectionRange && turretMode == false)
        {
            currentState = Calabaza.Chasing;
            playerDetected = true;
        }

        else if (angleToPlayer < detectionAngle / 2f && dirToPlayer.magnitude < detectionRange && turretMode == true)
        {
            currentState = Calabaza.Shooting;
            playerDetected = true;
        }
    }

    private void ChasePlayer()
    {
        Debug.Log("Chase");
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);


        if (Vector3.Distance(transform.position, player.position) < attackRange)
            currentState = Calabaza.Attacking;
        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            currentState = Calabaza.Patrolling;
            playerDetected = false;
        }

    }
    private void Shooting()
    {
        agent.speed = 0f;
                // Calcular la dirección hacia el jugador
        Vector3 dirToPlayer = player.transform.position - transform.position;
        dirToPlayer.y += 1.5f;
        // Rotar el objeto completo hacia esa dirección
        transform.rotation = Quaternion.LookRotation(dirToPlayer);
        if (playerDetected && Time.time >= nextFireTime)
        {
            //agent.enabled = false;
            
            ShootAtPlayer();
            nextFireTime = Time.time + 1f / fireRate;
        }
        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            currentState = Calabaza.Patrolling;
            playerDetected = false;
        }
    }
    private void AttackPlayer()
    {
        Debug.Log("Atacando al player");
        // Calcular la dirección hacia el jugador
        Vector3 dirToPlayer = player.transform.position - transform.position;
        dirToPlayer.y += 1.5f;
        // Rotar el objeto completo hacia esa dirección
        transform.rotation = Quaternion.LookRotation(dirToPlayer);
        if (Vector3.Distance(transform.position, player.position) > attackRange)
            currentState = Calabaza.Chasing;
    }
    private void ShootAtPlayer()
    {

        Debug.Log("Disparando");
        // Instantiate bullet prefab and shoot towards player
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void SetDestinationToNextPatrolPoint()
    {
        agent.speed = patrolSpeed;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
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





