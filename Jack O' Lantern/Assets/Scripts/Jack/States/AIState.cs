using Opsive.UltimateCharacterController.Game;
//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIState : MonoBehaviour
{
    #region Health Settings
    [Header("Health Settings")]
    public float maxHP = 100f;
    public float currentHP;
    int previousHP;
    public Slider sliderJack;

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
    public Transform player;
    private Vector3 startPosition;
    #endregion

    #region MeleeAttack
    [Header("MeleeAttack")]
    public Collider attackCollider;
    public float jumpDistance = 2f;
    #endregion

    #region InvokeEnemy Settings
    [Header("InvokeSettings")]
    public int maxEnemies = 10;
    public GameObject enemyPrefab;
    private bool _isInvoking = false;
    public Transform spawnPoint;
    public string enemyTag = "JackPumpkin";
    #endregion

    #region Tombs Settings
    [Header("Tombs Settings")]
    public List<GameObject> tombs = new List<GameObject>();
    public float teleportCooldown = 5f; // Tiempo de cooldown en segundos.
    private float lastTeleportTime;
    private GameObject lastTomb;
    public GameObject jackProjectile;
    public Transform jackMagicHand;
    public string objectTag = "Tomb";
    #endregion

    [SerializeField] private NavMeshAgent agent;
    private bool isFleeing = false;
    private float fleeTime = 0f;
    public  bool isAttacking = false;
    public bool isTeleported = false;

    public Animator anim;


    public static AIState instance;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        sliderJack = GetComponentInChildren<Slider>();
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
        if(sliderJack.value <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
    public void CheckStates()
    {
        //MI PERSONAJE ESTA A MITAD DE VIDA
        if(IsHalfLife())
        {
            //COMPROBAR SI HAY TUMBAS
            if (!CheckForTombs())
            {
                if (isInLongRange)
                {
                    if (CheckForMaxPumpkins())
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
                        if (!_isInvoking)
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
                if (CheckForMaxPumpkins())
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
                    if (!_isInvoking)
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
    

    public bool IsHalfLife()
    {
        if (sliderJack.value <= 0.5f)
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
        if (isFleeing || isAttacking || isTeleported) return;

        agent.SetDestination(player.position);
        SetAnimationState(1);
        Debug.Log("Estoy persiguiendo");
    }
    void Flee()
    {
        Debug.Log("Estoy huyendo");
        Vector3 direction = transform.position - player.position;
        Vector3 fleePosition = transform.position + direction.normalized * agent.speed;

        agent.SetDestination(fleePosition);
        SetAnimationState(2);
    }

    public void LookAt()
    {
        // Hacer que el enemigo siempre mire al jugador, pero solo en la dirección horizontal
        Vector3 playerPositionSameLevel = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(playerPositionSameLevel);
    }

    private void MeleeAttack()
    {
        isAttacking = true;
        Debug.Log("Estoy atacando");
        SetAnimationState(3);
        isAttacking = false;
        //Flee();
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
        agent.SetDestination(transform.position);
        _isInvoking = true;
        for (int i = 0; i < 2; i++)
        {
            SetAnimationState(5);
            // Espera hasta que la animación se complete
            yield return new WaitForSeconds(2.028f);

            // Invoca un enemigo
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // Asegúrate de asignar la etiqueta al enemigo invocado
            enemy.tag = enemyTag;


            // Espera un segundo antes de volver a invocar
            yield return new WaitForSeconds(1);
        }
        _isInvoking = false;

    }
    public void TeleportToTomb()
    {
        Debug.Log("TPEANDOSE");
        agent.SetDestination(transform.position);
        //if (tombs.Count == 0)
        //{
        //    Debug.LogError("No hay tumbas disponibles para teletransporte.");
        //    return;
        //}
        if (Time.time - lastTeleportTime >= teleportCooldown)
        {
            GameObject randomTomb;
            do
            {
                // Selecciona una tumba aleatoria.
                int randomIndex = Random.Range(0, tombs.Count);
                randomTomb = tombs[randomIndex];

            }
            while (randomTomb == lastTomb && tombs.Count > 1);

            // Teletransporta tu personaje a la tumba seleccionada.
            transform.position = randomTomb.transform.position;

            SetAnimationState(4);
            // Instancia el objeto en la posición de la tumba.
            Instantiate(jackProjectile, jackMagicHand.position, jackMagicHand.rotation);

            // Actualiza el tiempo del último teletransporte y la última tumba.
            lastTeleportTime = Time.time;
            lastTomb = randomTomb;
            isTeleported = true;
        }
        
    }

    public bool CheckForTombs()
    {
        if(tombs.Count <= 1)
        {
            Debug.Log("tombs false");
            return false;
        }
        else
        {
            return true;
        }
    }
    public void RemoveGameObject(GameObject obj)
    {
        tombs.Remove(obj);
    }
    void SetAnimationState(int state)
    {
        // Usa SetInt para cambiar el estado de la animación
        anim.SetInteger("State", state);
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

    void hit()
    {
        attackCollider.enabled = true;
    }
    void nohit()
    {
        attackCollider.enabled = false;
    }

}
