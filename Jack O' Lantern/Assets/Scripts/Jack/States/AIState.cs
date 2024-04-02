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

    #region
    [Header("BossChase")]
    public Transform player;
    public float speed = 5.0f;
    private Vector3 direction;
    private float chaosFactor = 2f;
    #endregion

    #region MeleeAttack
    [Header("MeleeAttack")]
    public Collider attackCollider;
    #endregion

    #region Invoke Settings
    [Header("InvokeSettings")]
    public GameObject enemyPrefab;
    private System.Random random = new System.Random();
    #endregion

    #region Tombs Settings
    [Header("Tombs Settings")]
    public List<GameObject> tombs;
    private bool isTeleporting = false;  // Nueva variable
    #endregion

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
        //HALF-LIFE
        if (currentHP <= maxHP / 2)
        {
            //SI HAY TUMBAS INICIA EL TP
            if (CheckForTombs() == true) 
            {
                TeleportToTomb();
            }
            //SI NO HAY TUMBAS EMPIEZA LA FASE
            else
            {
                //SI ESTOY EN RANGO DE INVOCACIÓN
                if (isInLongRange)
                {
                    //REVISA SI HAY UN MAX DE CALABAZAS
                    if (CheckForMaxPumpkins())
                    {
                        //REVISA SI ESTOY EN RANGO DE ATAQUE
                        if (isInMeleeRange)
                        {
                            //SI ESTOY EN RANGO DE ATAQUE Y ME BAJA LA VIDA HAZ UN DASH
                            if (CheckHealthDrop() == true)
                            {
                                Dash();
                            }
                            //SI ESTOY EN RANGO Y NO ME BAJA LA VIDA ATACA Y HAZ DASH
                            else
                            {
                                MeleeAttack();
                                Dash();
                            }
                        }
                        //SINO ESTOY EN RANGO DE ATAQUE PERSIGUE
                        else
                        {
                            Chase();
                        }
                    }
                    //SI NO HAY UN MAX DE CALABAZAS INVOCA
                    else
                    {
                        InvokeEnemies();
                    }
                }
                //SI NO ESTOY EN RANGO DE INVOCACIÓN REVISA SI ESTOY EN RANGO DE ATAQUE
                else
                {
                    //REVISA SI ESTOY EN RANGO DE ATAQUE 
                    if (isInMeleeRange)
                    {
                        //SI ESTOY EN RANGO DE ATAQUE Y RECIBO DAÑO DASHEA
                        if (CheckHealthDrop() == true)
                        {
                            Dash();
                        }
                        //SI NO RECIBO DAÑO HAZ EL ATAQUE Y DASHEA
                        else
                        {
                            MeleeAttack();
                            Dash();
                        }
                    }
                    //SINO ESTOY EN RANGO DE ATAQUE  PERSIGUE
                    else
                    {
                        Chase();
                    }
                }
            }
        }
        //FULL-LIFE
        else
        {
            if (isInMidRange)
            {
                if (isInMeleeRange)
                {
                    if (CheckHealthDrop())
                    {
                        Dash();
                    }
                    else
                    {
                        MeleeAttack();
                        Dash();
                    }
                }
                else
                {
                    Chase();
                }
            }
            else
            {
                if (CheckForMaxPumpkins())
                {
                    if (isInMeleeRange)
                    {
                        if (CheckHealthDrop())
                        {
                            Dash();
                        }
                        else
                        {
                          MeleeAttack();
                          Dash(); 
                        }
                    }
                }
                else
                {
                    InvokeEnemies();
                }
            }
        }
    }


    private bool AttackSucces()
    {
        throw new NotImplementedException();
    }

    private void Dash()
    {
        throw new NotImplementedException();
    }

    private void MeleeAttack()
    {

    }

    private bool CheckForMaxPumpkins()
    {
        var enemies = GameObject.FindGameObjectsWithTag("JackPumpkin");

        // Si hay 6 o más enemigos, devuelve true
        if (enemies.Length >= 6)
        {
            return true;
        }
        // Si no hay enemigos, devuelve false
        else if (enemies.Length == 0)
        {
            return false;
        }
        // En cualquier otro caso, puedes decidir qué hacer
        else
        {
            return false; // o true, dependiendo de lo que quieras
        }
    }


    IEnumerator InvokeEnemies()
    {
        GetComponent<CharacterController>().enabled = false;
        // Espera 3 segundos
        yield return new WaitForSeconds(3);

        // Invoca entre 2 y 3 enemigos
        int numEnemies = random.Next(2, 4);
        for (int i = 0; i < numEnemies; i++)
        {
            Instantiate(enemyPrefab, transform.position + Vector3.up * (i + 1), Quaternion.identity);
        }
        GetComponent<CharacterController>().enabled = true;
    }

    void Chase()
    {
        direction = (player.position - transform.position).normalized;

       
        direction += new Vector3(Mathf.Sin(Time.time * chaosFactor), 0, Mathf.Cos(Time.time * chaosFactor));

        
        direction = direction.normalized;

        
        transform.position += direction * speed * Time.deltaTime;

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
        if (previousHP - currentHP >= 30) 
        {
            Debug.Log("El personaje ha perdido 30 o más puntos de vida");
            return true;
        }
        else
        {
            return false;
        }
    }
    void UpdateTombs()
    {
        
        tombs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Tomb"));
    }

    public IEnumerator TeleportToTomb()
    {
        isTeleporting = true;  

        int randomTombIndex = UnityEngine.Random.Range(0, tombs.Count);

        
        Vector3 tombPosition = tombs[randomTombIndex].transform.position;

         transform.position = new Vector3(tombPosition.x, transform.position.y, tombPosition.z);

        yield return new WaitForSeconds(3);

        UpdateTombs();

        isTeleporting = false;  
    }

    public bool CheckForTombs()
    {
        UpdateTombs();

        if (tombs.Count > 0 && !isTeleporting)  {
            StartCoroutine(TeleportToTomb());
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
