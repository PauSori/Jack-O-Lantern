using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ZombieDetection : MonoBehaviour
{
    public float distance = 7;
    public float distanceAttack = 2;
    [Range(0, 360)] public float ViewAngle = 150f;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceAttack);
    }
}
