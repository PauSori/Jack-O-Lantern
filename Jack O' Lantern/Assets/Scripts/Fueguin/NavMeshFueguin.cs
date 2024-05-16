using Opsive.UltimateCharacterController.Character.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshFueguin : MonoBehaviour
{
    public Transform player;
    public List<Transform> fueguinPoints;
    public float followSpeed = 5f;
    public float followDistance = 2f;
    public float floatHeight = 0.5f;
    public float floatSpeed = 1f;
    public float teleportRange = 5f;

    private float originalY;
    private bool isTeleported = false;
    public float teleportTime = 2f;
    private float currentTime = 0f;

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

            int closestPointIndex = 0;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < fueguinPoints.Count; i++)
            {
                float distanceToFueguinPoint = Vector3.Distance(transform.position, fueguinPoints[i].position);
                if (distanceToFueguinPoint < closestDistance)
                {
                    closestDistance = distanceToFueguinPoint;
                    closestPointIndex = i;
                }
            }

            if (closestDistance <= teleportRange)
            {
                isTeleported = true;
                transform.position = fueguinPoints[closestPointIndex].position;
                Debug.Log("Diálogo " + (closestPointIndex + 1));
                fueguinPoints.RemoveAt(closestPointIndex);
            }
            else
            {
                FollowPlayer();
            }
        }
    }
}