using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackProjectile : MonoBehaviour
{
    public Transform target; // Objetivo del proyectil
    public float speed = 5f; // Velocidad del proyectil

    private void Start()
    {
        // Destruye el proyectil después de 6 segundos
        Destroy(gameObject, 6f);
    }

    private void Update()
    {
        if (target != null)
        {
            // Calcula la dirección del proyectil con un poco de desviación
            Vector3 direction = (target.position - transform.position).normalized;
            direction += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));

            // Mueve el proyectil
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
