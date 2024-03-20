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
    public bool isInMeleeRange = false;
    public bool isInMidRange = false;
    public bool isInLongRange = false;
    private float distanceToPlayer;
    private Transform playerTransform;

    [Header("BossChase")]
    public Transform player;
    public float speed = 5.0f;
    private Vector3 direction;
    private float chaosFactor = 2f;

    [Header("MeleeAttack")]
    public Collider attackCollider;


    [Header("Tombs Settings")]
    public List<GameObject> tombs;
    private bool isTeleporting = false;  // Nueva variable

    public void Start()
    {
        currentHP = maxHP;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        attackCollider.enabled = false;
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
        //if (isInMeleeRange)
        //{
        //    MeleeAttack();
        //}
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

       
        direction += new Vector3(Mathf.Sin(Time.time * chaosFactor), 0, Mathf.Cos(Time.time * chaosFactor));

        
        direction = direction.normalized;

        
        transform.position += direction * speed * Time.deltaTime;

    }
    //void MeleeAttack()
    //{
    //    attackCollider.enabled = true;

    //    // Desactiva el collider después de 1 segundo
    //    Invoke("DeactivateAttack", 1.0f);
    //}
    //void DeactivateAttack()
    //{
    //    // Desactiva el collider
    //    attackCollider.enabled = false;
    //}
    //void OnTriggerEnter(Collider other)
    //{
    //    // Comprueba si el collider tocado tiene el tag "Player"
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("El jugador ha sido tocado por el ataque");
    //    }
    //}
    public void CheckForRanges()
    {
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        isInMeleeRange = false;
        isInMidRange = false;
        isInLongRange = false;

        if (distanceToPlayer <= meleeRange)
        {
            isInMeleeRange = true;
            // Implementa aquí la lógica de ataque cuerpo a cuerpo
        }
        else if (distanceToPlayer <= midRange)
        {
            isInMidRange = true;
            // Implementa aquí la lógica de ataque a media distancia
        }
        else if (distanceToPlayer <= longRange)
        {
            isInLongRange = true;
            // Implementa aquí la lógica de ataque a larga distancia
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
