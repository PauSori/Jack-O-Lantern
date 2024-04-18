using System;
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
    public GameObject enemyPrefab;
    public List<GameObject> calabazas = new List<GameObject>();
    #endregion

    #region Tombs Settings
    [Header("Tombs Settings")]
    public List<GameObject> tombs;
    private bool isTeleporting = false;  // Nueva variable
    #endregion

    public Animator anim;

    //COSAS QUE FALTAN
    //ATAQUE
    //SALTO HACIA ATRAS

    public void Start()
    {
        currentHP = maxHP;
        startPosition = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
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
                    if (CheckForMaxPumpkins())
                    {
                        if (isInMeleeRange)
                        {
                                MeleeAttack();
                                Dash();
                        }
                        else
                        {
                            Chase();
                        }
                    }
                    else
                    {
                        InvokeEnemies();
                    }
                }
                else
                {
                    if (isInMeleeRange)
                    {
                            MeleeAttack();
                            Dash();
                        
                    }
                    else 
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
                if(CheckForMaxPumpkins() )
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
                    InvokeEnemies();
                }
            }
            else
            {
                if (isInMeleeRange)
                {
                  MeleeAttack();
                  Dash();
                }
                else if (isInMidRange)
                {
                    Chase();
                }
            }
        }
    }
    private void Dash()
    {
        Debug.Log("DASH");
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

    private void MeleeAttack()
    {
        Debug.Log("ATAQUE A MELEE");
    }

    private bool CheckForMaxPumpkins()
    {
        int cantidadDeCalabazas = calabazas.Count;

        if (cantidadDeCalabazas >= 5 && cantidadDeCalabazas <= 6)
        {
            Debug.Log("A tope de calabazas");
            return true;
        }
        else
        {
            if (cantidadDeCalabazas < 5)
            {
                Debug.Log("Puedo invocar");
            }
            return false;
        }
    }
    public void  InvokeEnemies()
    {
        Debug.Log("INVOCANDO");
    }
    void Chase()
    {
        Vector3 direction = player.position - transform.position;
        float zigzag = Mathf.Sin(Time.time * zigzagSpeed) * zigzagDistance;

        transform.position += (direction.normalized + new Vector3(zigzag, 0, 0)) * speed * Time.deltaTime;

        anim.SetFloat("Zigzag", zigzag / zigzagDistance);
    }

    public void LookAt()
    {
        // Hacer que el enemigo siempre mire al jugador, pero solo en la dirección horizontal
        Vector3 playerPositionSameLevel = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(playerPositionSameLevel);
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

    bool CheckHealthDrop()
    {
        if (previousHP - currentHP >= 10) 
        {
            Debug.Log("El personaje ha perdido 30 o más puntos de vida");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TeleportToTomb()
    {
        Debug.Log("TP A TUMBAS");
    }

    public bool CheckForTombs()
    {
        if (GameManager.instance.tombs.Count > 0 && !isTeleporting)
        {
            TeleportToTomb();
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
