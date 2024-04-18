using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcoController : MonoBehaviour
{
    // Referencias a objetos en la escena
    public Transform arcoTransform; // Transform del modelo del arco
    public GameObject flechaPrefab; // Prefab de la flecha
    public Transform puntoDisparo; // Punto desde donde se disparan las flechas

    // Parámetros del arco
    public float fuerzaDisparo = 20f; // Fuerza de disparo
    public float tiempoRecarga = 1f; // Tiempo de recarga entre disparos

    private bool puedeDisparar = true;

    private void Update()
    {
        // Detectar entrada del jugador (por ejemplo, clic izquierdo)
        if (Input.GetButtonDown("Fire1") && puedeDisparar)
        {
            DispararFlecha();
        }
    }

    private void DispararFlecha()
    {
        // Crear una nueva flecha
        GameObject nuevaFlecha = Instantiate(flechaPrefab, puntoDisparo.position, Quaternion.identity);
        Rigidbody rbFlecha = nuevaFlecha.GetComponent<Rigidbody>();

        // Aplicar fuerza al Rigidbody de la flecha
        rbFlecha.AddForce(arcoTransform.forward * fuerzaDisparo, ForceMode.Impulse);

        // Iniciar temporizador de recarga
        StartCoroutine(RecargarArco());
    }

    private IEnumerator RecargarArco()
    {
        puedeDisparar = false;
        yield return new WaitForSeconds(tiempoRecarga);
        puedeDisparar = true;
    }
}