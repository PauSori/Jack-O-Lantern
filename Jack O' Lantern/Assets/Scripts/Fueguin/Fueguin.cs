using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fueguin : MonoBehaviour
{
    public Transform player; // El personaje principal
    public float speed = 2.0f; // La velocidad a la que el fuego fatuo sigue al personaje principal
    public Vector3 offset = new Vector3(1, 0, 0); // El desplazamiento desde el personaje principal

    void Update()
    {
        // Calcular la nueva posición del fuego fatuo
        Vector3 newPos = player.position + offset;

        // Interpolar suavemente desde la posición actual del fuego fatuo hasta la nueva posición
        transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
    }
}
