using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    public GameObject parentObject;

    void Start()
    {

    }

    void Update()
    {
        // Tu l�gica de comportamiento para los enemigos
        // ...

        // Si el enemigo debe ser destruido, llamar a DestroyEnemy()
        if (Input.GetKeyDown(KeyCode.P))
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
        PuertaController.instance.RemoveGameObject(parentObject);
        Destroy(parentObject);
        Destroy(gameObject);
    }


}