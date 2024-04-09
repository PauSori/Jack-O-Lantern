using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fueguin : MonoBehaviour
{
    public Transform player;
    public float speed = 2.0f;
    public Vector3 offset = new Vector3(1, 0, 0);
    public bool CheckCombat = false;
    public bool objectInArea = false;
    private bool isCooldown = false;
    public float detectionRadius = 8.0f;
    public float detectionObjectRadius = 18.0f;
    public LayerMask Enemy;
    public LayerMask objects;
    public bool areEnemiesNearby = false;
    public Collider closestEnemy = null;
    private Collider closestObject;

    void Update()
    {
        CheckStates();
    }

    void CheckStates()
    {
        if (CheckCombatState() == true)
        {
            if (CheckCD() == false)
            {
                if (CheckEnemy() == true)
                {
                    Stun();
                    FollowEnemy(); // Mueve hacia el enemigo aquí
                }
                else
                {
                    FollowPlayer(); // Si no hay enemigos, sigue al jugador
                }
            }
        }
        else
        {
            if (CheckObjectInArea() == true)
            {
                FollowObject();
            }
            else
            {
                FollowPlayer();
            }
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
            FollowPlayer(); // Si el objeto desaparece, sigue al jugador
        }
    }

    private bool CheckEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius, Enemy);

        if (enemies.Length > 0)
        {
            areEnemiesNearby = true;
            float closestDistanceSqr = Mathf.Infinity;

            foreach (Collider enemy in enemies)
            {
                Vector3 directionToEnemy = enemy.transform.position - transform.position;
                float dSqrToEnemy = directionToEnemy.sqrMagnitude;

                if (dSqrToEnemy < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToEnemy;
                    closestEnemy = enemy;
                }
            }

            Debug.Log("El enemigo más cercano es: " + closestEnemy.gameObject.name);
        }
        else
        {
            areEnemiesNearby = false;
            closestEnemy = null;
        }

        return areEnemiesNearby;
    }

    private bool CheckCD()
    {
        return isCooldown;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Objeto"))
        {
            //if layer == enemy crides a checkenemy
            Debug.Log("Objeto detectado!");
            // CheckEnemy();
            objectInArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Objeto"))
        {
            objectInArea = false;
        }
    }

    private bool CheckObjectInArea()
    {
        Collider[] Object = Physics.OverlapSphere(transform.position, detectionObjectRadius, objects);
        //Debug.Log("Caja");
        return objectInArea;
    }

    private void FollowPlayer()
    {
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

    public bool CheckCombatState()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            CheckCombat = !CheckCombat;
        }
        return CheckCombat;
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(5);
        isCooldown = false;
    }

    private void Stun()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(Cooldown());
            FollowPlayer(); // Agrega esta línea
        }
    }
    void OnDrawGizmosSelected()
    {
        // Dibuja una esfera roja para el rango melee
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionObjectRadius);
    }
}

//Podran haber problemas
//Como que Fueguin se puede ir a un enemigo sin que te des cuenta y que se quede ahi para siempre hasta que lo mates