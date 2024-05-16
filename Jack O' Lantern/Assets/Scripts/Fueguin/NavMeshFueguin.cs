using Opsive.UltimateCharacterController.Character.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshFueguin : MonoBehaviour
{
    public Transform player;
    public List<Transform> fueguinPoints; // Lista de puntos a los que se teletransportará
    public float followSpeed = 5f;
    public float followDistance = 2f;
    public float floatHeight = 0.5f;
    public float floatSpeed = 1f;
    public float teleportRange = 5f; // Rango para teletransportarse

    private float originalY;
    private bool isTeleported = false; // Para saber si se ha teletransportado
    private float teleportTime = 2f; // Tiempo que estará teletransportado
    private float currentTime = 0f; // Contador de tiempo
    private int currentPointIndex = 0; // Índice del punto actual

    void Start()
    {
        originalY = transform.position.y;
    }

    void Update()
    {
        Teleport();
    }

    public void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        Vector3 desiredPosition = player.position - direction * followDistance;

        desiredPosition.y = originalY + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
    public void Teleport()
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
            float distanceToFueguinPoint = Vector3.Distance(transform.position, fueguinPoints[currentPointIndex].position);
            if (distanceToFueguinPoint <= teleportRange)
            {
                isTeleported = true;
                transform.position = fueguinPoints[currentPointIndex].position; // Se teletransporta al punto
                Debug.Log("Diálogo " + (currentPointIndex + 1));
                fueguinPoints.RemoveAt(currentPointIndex); // Elimina el punto de la lista
                if (fueguinPoints.Count > 0) // Si aún quedan puntos, pasa al siguiente
                {
                    currentPointIndex %= fueguinPoints.Count;
                }
            }
            else
            {
                FollowPlayer();
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, teleportRange);
    }

}
