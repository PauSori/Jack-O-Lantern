using Opsive.UltimateCharacterController.Character.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshFueguin : MonoBehaviour
{
    public Transform player;
    public Transform fueguinPoint; // Añade el punto al que se teletransportará
    public float followSpeed = 5f;
    public float followDistance = 2f;
    public float floatHeight = 0.5f;
    public float floatSpeed = 1f;
    public float teleportRange = 5f; // Rango para teletransportarse

    private float originalY;
    private bool isTeleported = false; // Para saber si se ha teletransportado
    private float teleportTime = 10f; // Tiempo que estará teletransportado
    private float currentTime = 0f; // Contador de tiempo

    void Start()
    {
        originalY = transform.position.y;
    }

    void Update()
    {
        if (isTeleported)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= teleportTime)
            {
                isTeleported = false;
                currentTime = 0f;
            }
        }
        else
        {
            // Calcula la distancia al punto
            float distanceToFueguinPoint = Vector3.Distance(transform.position, fueguinPoint.position);
            if (distanceToFueguinPoint <= teleportRange)
            {
                isTeleported = true;
                transform.position = fueguinPoint.position; // Se teletransporta al punto
            }
            else
            {
                FollowPlayer();
            }
        }
    }

    public void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        Vector3 desiredPosition = player.position - direction * followDistance;

        desiredPosition.y = originalY + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
    void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, teleportRange);
    }

}
