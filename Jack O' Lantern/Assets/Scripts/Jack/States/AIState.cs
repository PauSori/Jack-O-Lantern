using Opsive.UltimateCharacterController.Game;
//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AIState : MonoBehaviour
{
    #region Health Settings
    [Header("Health Settings")]
    public float maxHP = 100f;
    public float currentHP;
    int previousHP;
    #endregion

    #region Detection Ranges
    [Header("Detection Ranges")]
    public float meleeRange;
    public float midRange;
    public float longRange;
    public bool isInMeleeRange = false;
    public bool isInMidRange = false;
    public bool isInLongRange = false;
    private float distanceToPlayer;
    private Transform playerTransform;
    #endregion

    #region BossChase
    [Header("BossChase")]
    public float speed = 2f;
    public float zigzagSpeed = 2f;
    public float zigzagDistance = 2f;
    public Transform player;
    private Vector3 startPosition;
    #endregion

    #region MeleeAttack
    [Header("MeleeAttack")]
    public Collider attackCollider;
    public float jumpDistance = 10f;
    #endregion

    #region InvokeEnemy Settings
    [Header("InvokeSettings")]
    public int maxEnemies = 10;
    public GameObject enemyPrefab;
    private bool isInvoking = false;
    public Transform spawnPoint;
    public string enemyTag = "JackPumpkin";
    #endregion

    #region Tombs Settings
    [Header("Tombs Settings")]
    public GameObject[] tombs;
    public float teleportCooldown = 5f; // Tiempo de cooldown en segundos.
    private float lastTeleportTime;
    private GameObject lastTomb;
    #endregion

    public Animator anim;


    public void Start()
    {
        currentHP = maxHP;
        startPosition = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        lastTeleportTime = -teleportCooldown;
        attackCollider.enabled = false;
    }
    private void Update()
    {
        CheckStates();
        CheckForRanges();
        LookAt();
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
        //MI PERSONAJE ESTA A MITAD DE VIDA
        if(IsHalfLife(currentHP, maxHP))
        {
            //COMPROBAR SI HAY TUMBAS
            if (!CheckForTombs())
            {
                if (isInLongRange)
                {
                    if (!CheckForMaxPumpkins())
                    {
                        if (isInMeleeRange)
                        {
                            MeleeAttack();
                        }
                        else if (isInMidRange)
                        {
                            Chase();
                        }
                    }
                    else
                    {
                        StartCoroutine(InvokeEnemies());
                    }
                }
                else
                {
                    if (isInMeleeRange)
                    {
                        MeleeAttack();
                    }
                    else if (isInMidRange)
                    {
                        Chase();
                    }
                }
            }
            else
            {
                TeleportToTomb();
            }
        }
        //ESTOY A FULL VIDA
        else
        {
            if (isInLongRange)
            {
                if(CheckForMaxPumpkins())
                {
                    if (isInMeleeRange)
                    {
                            MeleeAttack();
                    }
                    else if(isInMidRange)
                    {
                        Chase();
                    }
                }
                else 
                {
                    if (!isInvoking)
                    {
                        StartCoroutine(InvokeEnemies());
                    }
                }
            }
            else
            {
                if (isInMeleeRange)
                {
                  MeleeAttack();
                }
                else if (isInMidRange)
                {
                    Chase();
                }
            }
        }
    }
    public bool IsHalfLife(float vidaActual, float vidaTotal)
    {
        if (vidaActual <= vidaTotal / 2)
        {
            return true; // El personaje está a mitad de vida o menos
        }
        else
        {
            return false; // El personaje tiene más de la mitad de vida
        }
    }

    public void CheckForRanges()
    {
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        isInMeleeRange = false;
        isInMidRange = false;
        isInLongRange = false;

        if (distanceToPlayer <= meleeRange)
        {
            isInMeleeRange = true;
        }
        else if (distanceToPlayer <= midRange)
        {
            isInMidRange = true;
        }
        else if (distanceToPlayer <= longRange)
        {
            isInLongRange = true;
        }
    }

    void Chase()
    {
        Vector3 direction = player.position - transform.position;
        float zigzag = Mathf.Sin(Time.time * zigzagSpeed) * zigzagDistance;

        transform.position += (direction.normalized + new Vector3(zigzag, 0, 0)) * speed * Time.deltaTime;

    }

    public void LookAt()
    {
        // Hacer que el enemigo siempre mire al jugador, pero solo en la dirección horizontal
        Vector3 playerPositionSameLevel = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(playerPositionSameLevel);
    }

    private void MeleeAttack()
    {
        anim.Play("Melee_Attack");

        // Calcula la posición de teletransporte
        Vector3 posicionTP = transform.position - transform.forward * jumpDistance;

        // Teletransporta a tu personaje
        transform.position = posicionTP;
    }

    private bool CheckForMaxPumpkins()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("JackPumpkin");

        // Si el número de enemigos es mayor que el máximo, devuelve false
        if (enemies.Length >= maxEnemies)
        {
            return true;
        }
        // Si no, devuelve false
        else
        {
            return false;
        }
    }
    IEnumerator InvokeEnemies()
    {
        isInvoking = true;
        for (int i = 0; i < 2; i++)
        {
            // Invoca un enemigo
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // Asegúrate de asignar la etiqueta al enemigo invocado
            enemy.tag = enemyTag;

            // Espera un segundo antes de volver a invocar
            yield return new WaitForSeconds(4);
        }
        isInvoking = false;
    }
    

    public void TeleportToTomb()
    {
        if (Time.time - lastTeleportTime >= teleportCooldown)
        {
            GameObject randomTomb;
            do
            {
                // Selecciona una tumba aleatoria.
                int randomIndex = Random.Range(0, tombs.Length);
                randomTomb = tombs[randomIndex];
            }
            while (randomTomb == lastTomb); // Continúa seleccionando una tumba aleatoria hasta que no sea la misma que la última.

            // Teletransporta tu personaje a la tumba seleccionada.
            transform.position = randomTomb.transform.position;

            // Actualiza el tiempo del último teletransporte y la última tumba.
            lastTeleportTime = Time.time;
            lastTomb = randomTomb;
        }
    }

    public bool CheckForTombs()
    {
        // Supongamos que 'Tomb' es el tag que has asignado a las tumbas en tu juego.
        GameObject[] tombs = GameObject.FindGameObjectsWithTag("Tomb");

        if (tombs.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
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
