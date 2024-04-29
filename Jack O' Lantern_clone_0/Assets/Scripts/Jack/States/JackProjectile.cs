using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackProjectile : MonoBehaviour
{
    public float speed = 5.0f; // Velocidad a la que el enemigo se mueve hacia el jugador
    public float sidewaySpeed = 1.0f; // Velocidad a la que el enemigo se mueve de lado a lado
    private Vector3 direction;
    private Transform player; // Jugador

    void Start()
    {
        // Busca al jugador en la escena utilizando su etiqueta
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Inicializa la dirección de movimiento lateral
        direction = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized;
    }

    void Update()
    {
        // Mueve el enemigo hacia el jugador
        Vector3 playerDirection = (player.position - transform.position).normalized;
        transform.position += playerDirection * speed * Time.deltaTime;

        // Mueve el enemigo de lado a lado
        transform.position += direction * sidewaySpeed * Time.deltaTime;

        // Cambia la dirección de movimiento lateral aleatoriamente
        if (Random.value < 0.01f) // Cambia la dirección aproximadamente una vez cada 100 frames
        {
            direction = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized;
        }
    }
}
