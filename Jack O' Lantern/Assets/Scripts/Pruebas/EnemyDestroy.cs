using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {
        // Tu lógica de comportamiento para los enemigos
        // ...

        // Si el enemigo debe ser destruido, llamar a DestroyEnemy()
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDestroy();
        }
        if(this.gameObject == null)
        {
            OnDestroy();
        }
    }

    void OnDestroy()
    {
        PuertaController.instance.RemoveGameObject(gameObject);
        Destroy(gameObject);
    }


}
