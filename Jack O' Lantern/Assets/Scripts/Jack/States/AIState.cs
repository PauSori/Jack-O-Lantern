using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AIState : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHP = 100f;
    public float currentHP;

    [Header("Detection Ranges")]
    public float meleeRange;
    public float midRange;
    public float longRange;
    private Transform playerTransform;
    private bool isInMeleeRange = false;
    private bool isInMidRange = false;
    private bool isInLongRange = false;

    [Header("BossChase")]
    public Transform player;
    public float speed = 5.0f;
    private Vector3 direction;

    [Header("Tombs Settings")]
    public List<GameObject> tombs;
    private bool isTeleporting = false;  // Nueva variable

    public void Start()
    {
        currentHP = maxHP;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    private void Update()
    {
        CheckStates();

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentHP -= 10;
            if (currentHP < 0)
            {
                currentHP = 0;
            }
        }
    }

    public void CheckStates()
    {
        if(currentHP > maxHP / 2)
        {
            FullLifeState();
        }
        if (currentHP <= maxHP / 2)
        {
            HalfLifeState();
        }
        if (isInMidRange)
        {
            Chase();
        }
    }

    void HalfLifeState()
    {
        CheckForTombs();
    }
    void FullLifeState() 
    {
        CheckForRanges();
    }
    void Chase()
    {
        direction = (player.position - transform.position).normalized;

        // Add a diagonal movement
        direction += new Vector3(Mathf.Sin(Time.time), 0, Mathf.Cos(Time.time));

        // Normalize the direction
        direction = direction.normalized;

        // Move the enemy
        transform.position += direction * speed * Time.deltaTime;
    }
    public void CheckForRanges()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= meleeRange)
        {
            isInMeleeRange = true;
            isInMidRange = false;
            isInLongRange = false;
            // Implementa aquí la lógica de ataque cuerpo a cuerpo
        }
        else if (distanceToPlayer <= midRange)
        {
            isInMeleeRange = false;
            isInMidRange = true;
            isInLongRange = false;
            // Implementa aquí la lógica de ataque a media distancia
        }
        else if (distanceToPlayer <= longRange)
        {
            isInMeleeRange = false;
            isInMidRange = false;
            isInLongRange = true;
            // Implementa aquí la lógica de ataque a larga distancia
        }
        else
        {
            isInMeleeRange = false;
            isInMidRange = false;
            isInLongRange = false;
        }
    }
    void UpdateTombs()
    {
        // Busca todos los objetos en la escena con la etiqueta "Tomb"
        tombs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Tomb"));
    }

    public IEnumerator TeleportToTomb()
    {
        isTeleporting = true;  // Establece isTeleporting a true al inicio de la corrutina

        int randomTombIndex = UnityEngine.Random.Range(0, tombs.Count);

        // Obtiene la posición de la tumba seleccionada
        Vector3 tombPosition = tombs[randomTombIndex].transform.position;

        // Teletransporta al personaje a la tumba seleccionada, manteniendo la misma posición Y
        transform.position = new Vector3(tombPosition.x, transform.position.y, tombPosition.z);

        // Espera durante el tiempo de retardo
        yield return new WaitForSeconds(3);

        // Actualiza la lista de tumbas
        UpdateTombs();

        isTeleporting = false;  // Establece isTeleporting a false al final de la corrutina
    }

    public void CheckForTombs()
    {
        UpdateTombs();

        if (tombs.Count > 0 && !isTeleporting)  // Comprueba si isTeleporting es false antes de iniciar la corrutina
        {
            StartCoroutine(TeleportToTomb());
        }
        else
        {
            //CheckSummonRange();
        }
    }
    void OnDrawGizmosSelected()
    {
        // Dibuja una esfera roja para el rango melee
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        // Dibuja una esfera amarilla para el rango medio
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, midRange);

        // Dibuja una esfera verde para el rango largo
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, longRange);
    }

}
