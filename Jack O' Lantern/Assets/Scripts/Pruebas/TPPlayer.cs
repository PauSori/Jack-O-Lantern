using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPPlayer : MonoBehaviour
{
    public Transform targetPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Teleport");
            other.transform.position = targetPosition.position;
        }
    }
}
