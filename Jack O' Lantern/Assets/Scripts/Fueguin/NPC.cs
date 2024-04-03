using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float velocidad = 10f;
    private void Update()
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }
}
