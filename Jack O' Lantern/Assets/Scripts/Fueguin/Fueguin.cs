using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fueguin : MonoBehaviour
{
    public GameObject panelSeñal;

    public Transform player;
    public float speed = 2.0f;
    public Vector3 offset = new Vector3(1, 0, 0);
    public bool CheckCombat = false;
    public bool objectInArea = false;
    private bool isCooldown = false;
    public float detectionRadius = 8.0f;
    public float detectionObjectRadius = 18.0f;
    public float maxDistanceFromPlayer = 30.0f;
    public LayerMask Enemy;
    public LayerMask Objeto;
    public bool areEnemiesNearby = false;
    private Collider closestEnemy = null;
    private Collider closestObject;
    public GameObject Player; // NUEVO IMPLEMENTADO // Nueva variable para la distancia máxima

    private void Start()
    {
        panelSeñal.SetActive(false);
    }
    void Update()
    {
        CheckStates();
    }

    void CheckStates()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > maxDistanceFromPlayer)
        {

            FollowPlayer();
        }
        else if (CheckEnemy() == true) // Si detecta un enemigo, entra en modo de combate
        {
            CheckCombat = true;

            if (CheckCD() == false)
            {
                Stun();
                FollowEnemy();
            }
        }
        else if (CheckObjectInArea() == true) // Si no hay enemigos, verifica si hay un objeto cerca
        {
            CheckCombat = false;
            FollowObject();
        }
        else
        {
            CheckCombat = false;
            //Debug.Log("LLendo al player");
            FollowPlayer();
        }
    }

    ////////////////// Hacer que a la que detecte el objeto no vaya y vuelva todo el rato

    private bool CheckEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius, Enemy);

        if (enemies.Length > 0)
        {
            areEnemiesNearby = true;
            float closestDistanceSqr = Mathf.Infinity;
            foreach (Collider enemy in enemies)
            {
                float distanceSqr = (enemy.transform.position - transform.position).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestEnemy = enemy;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckObjectInArea()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        Collider[] objects = Physics.OverlapSphere(transform.position, detectionObjectRadius, Objeto);
       
        if (objects.Length > 0 && distanceToPlayer < maxDistanceFromPlayer)
        {
            Debug.Log("Objeto encontrado");
            Debug.Log("Distancetoplayer:" + distanceToPlayer);
            panelSeñal.SetActive(true);
            objectInArea = true;
            float closestDistanceSqr = Mathf.Infinity;
            foreach (Collider obj in objects)
            {
                float distanceSqr = (obj.transform.position - transform.position).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestObject = obj;
                }
            }
            return true;
        }
        else
        {
            Debug.Log("Sin objeto");

            objectInArea = false;
            return false;
        }
    }
    
    private void FollowObject()
    {
        if (closestObject != null)
        {
            Vector3 newPos = closestObject.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
        }
        else
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        Debug.Log("dins follow player");
        panelSeñal.SetActive(false);
        Vector3 newPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
    }

    private void FollowEnemy()
    {
        if (closestEnemy != null)
        {
            Vector3 newPos = closestEnemy.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
        }
        else
        {
            FollowPlayer(); // Si el enemigo desaparece, sigue al jugador
        }
    }

    private void Stun()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(Cooldown());
            FollowPlayer(); // Agrega esta línea
        }
    }


    private bool CheckCD()
    {
        return isCooldown;
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(5);
        isCooldown = false;
    }

    
    //Areas
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionObjectRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxDistanceFromPlayer);
    }
}