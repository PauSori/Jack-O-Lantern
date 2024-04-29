using Opsive.UltimateCharacterController.Game;
//using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class AIState : MonoBehaviour
{
    // Health Settings
    [Header("Health Settings")]
    public int maxHP = 100;
    private int currentHP;

    // Detection Ranges
    [Header("Detection Ranges")]
    public float meleeRange;
    public float midRange;
    public float longRange;
    public bool isInMeleeRange;
    public bool isInMidRange;
    public bool isInLongRange;
    private Transform playerTransform;
    public GameObject jack;

    // Boss Chase
    [Header("Boss Chase")]
    public float speed = 2f;

    // Melee Attack
    [Header("Melee Attack")]
    public Collider attackCollider;
    public float jumpDistance = 5f;

    // Invoke Enemy Settings
    [Header("Invoke Settings")]
    public int maxEnemies = 10;
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public string enemyTag = "JackPumpkin";

    // Tombs Settings
    [Header("Tombs Settings")]
    public List<GameObject> tombs;
    public float teleportCooldown = 5f;
    private float lastTeleportTime;
    private bool isTeleportingToTomb = false;
    private GameObject lastTomb;
    public GameObject jackProjectile;
    public GameObject jackMagicHand;

    private Animator anim;

    private bool isTeleporting;
    private bool isInvoking;

    private bool isDead = false;

    private void Start()
    {
        currentHP = maxHP;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        lastTeleportTime = -teleportCooldown;
        attackCollider.enabled = false;
    }

    private void Update()
    {
        if (!isDead)
        {
            // Solo realizar acciones si el personaje no está muerto
            CheckStates();
            LookAt();
            CheckForRanges();
            if (Input.GetKeyDown(KeyCode.E))
            {
                TakeDamage(10);
            }
        }
    }

    private void CheckStates()
    {
        if (IsHalfLife())
        {
            if (!CheckForTombs())
            {
                if (isInLongRange)
                {
                    if (!CheckForMaxPumpkins())
                    {
                        if (isInMeleeRange)
                        {
                            StartCoroutine(MeleeAttack());
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
                        StartCoroutine(MeleeAttack());
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
        else
        {
            if (isInLongRange)
            {
                if (CheckForMaxPumpkins())
                {
                    if (isInMeleeRange)
                    {
                        StartCoroutine(MeleeAttack());
                    }
                    else if (isInMidRange)
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
                    StartCoroutine(MeleeAttack());
                }
                else if (isInMidRange)
                {
                    Chase();
                }
            }
        }
    }

    private bool IsHalfLife()
    {
        return currentHP <= maxHP / 2;
    }

    private void TakeDamage(int amount)
    {
        if (!isDead)
        {
            currentHP -= amount;
            if (currentHP <= 0)
            {
                Die(); // Llamar a la función de muerte si la vida llega a cero o menos
            }
        }
    }
    private void Die()
    {
        isDead = true; // Marcar al personaje como muerto
        SetAnimationState(7); // Reproducir la animación de muerte

        // Desactivar todos los controles y comportamientos
        isInMeleeRange = false;
        isInMidRange = false;
        isInLongRange = false;

        // Deshabilitar cualquier otro comportamiento que pueda tener el personaje

        // Esperar a que termine la animación de muerte antes de destruir el GameObject
        StartCoroutine(DestroyAfterAnimation());
    }
    private IEnumerator DestroyAfterAnimation()
    {
        // Esperar a que la animación de muerte termine
        yield return new WaitForSeconds(1);

        // Destruir el GameObject actual
        Destroy(gameObject);
    }
    private void CheckForRanges()
    {
        if (!isTeleporting)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            isInMeleeRange = distanceToPlayer <= meleeRange;
            isInMidRange = distanceToPlayer > meleeRange && distanceToPlayer <= midRange;
            isInLongRange = distanceToPlayer > midRange && distanceToPlayer <= longRange;

            attackCollider.enabled = isInMeleeRange;
        }

    }

    private void Chase()
    {
        Vector3 direction = playerTransform.position - transform.position;
        transform.position += direction.normalized * speed * Time.deltaTime;
        SetAnimationState(1);
    }

    private void LookAt()
    {
        Vector3 playerPositionSameLevel = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        transform.LookAt(playerPositionSameLevel);
    }

    private IEnumerator MeleeAttack()
    {
        SetAnimationState(4);
        yield return new WaitForSeconds(1);

        SetAnimationState(3);
        yield return new WaitForSeconds(1);

        Teleport(jack, jumpDistance);
    }

    private void Teleport(GameObject character, float distance)
    {
        isTeleporting = true;
        SetAnimationState(6);

        Vector3 currentPosition = character.transform.position;
        Vector3 newPosition = currentPosition - character.transform.forward * distance;
        character.transform.position = newPosition;

        isTeleporting = false;
    }

    private bool CheckForMaxPumpkins()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        return enemies.Length >= maxEnemies;
    }

    private IEnumerator InvokeEnemies()
    {
        isInvoking = true;
        for (int i = 0; i < 2; i++)
        {
            SetAnimationState(2);
            yield return new WaitForSeconds(2.028f);

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemy.tag = enemyTag;

            yield return new WaitForSeconds(1);
        }
        isInvoking = false;
    }

    private void TeleportToTomb()
    {
        isTeleportingToTomb = true;
        if (Time.time - lastTeleportTime >= teleportCooldown)
        {
            SetAnimationState(5);

            GameObject randomTomb;
            do
            {
                int randomIndex = Random.Range(0, tombs.Count);
                randomTomb = tombs[randomIndex];
            }
            while (randomTomb == lastTomb);

            transform.position = randomTomb.transform.position;
            StartCoroutine(InstantiateProjectiles());

            lastTeleportTime = Time.time;
            lastTomb = randomTomb;
        }
        
    }

    private IEnumerator InstantiateProjectiles()
    {
        for (int i = 0; i < 2; i++)
        {
            Instantiate(jackProjectile, jackMagicHand.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
    }

    public bool CheckForTombs()
    {
        if (tombs.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnDestroy()
    {
        // Elimina esta tumba de la lista cuando se destruye
        tombs.Remove(this.gameObject);
    }

    private void SetAnimationState(int state)
    {
        anim.SetInteger("State", state);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, midRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, longRange);
    }
}
