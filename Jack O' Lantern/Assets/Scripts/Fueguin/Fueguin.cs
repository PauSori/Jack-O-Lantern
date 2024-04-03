using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fueguin : MonoBehaviour
{
    public Transform player; // El personaje principal
    public float speed = 2.0f; // La velocidad a la que el fuego fatuo sigue al personaje principal
    public Vector3 offset = new Vector3(1, 0, 0); // El desplazamiento desde el personaje principal
    public bool CheckCombat = false;
    public bool objectInArea = false;

    void Update()
    {
        FollowPlayer();
        CheckStates();

    }

    void CheckStates()
    {
        if (CheckCombatState() == true)
        {
            if (CheckCD() == false)
            {
                FollowPlayer();
            }
            else
            {
                
                if (CheckEnemy() == true)
                {
                    Stun();
                }
                else
                {
                    FollowPlayer();

                }
            }
        }
        else
        {
            if (CheckObjectInArea() == true)
            {
                FueguinPointing();
            }
            else
            {
                FollowPlayer();
            }
        }
      
    }

    private void FueguinPointing()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que entró en el área es el que estás buscando
        if (other.gameObject.CompareTag("Objeto"))
        {
            Debug.Log("Objeto detectado!");
            objectInArea = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        // Comprueba si el objeto que salió del área es el que estás buscando
        if (other.gameObject.CompareTag("Objeto"))
        {
            Debug.Log("Objeto se ha ido!");
            objectInArea = false;
        }
    }
    private bool CheckObjectInArea()
    {
        return objectInArea;
    }

    private void Stun()
    {
        
    }

    private bool CheckEnemy()
    {
        throw new NotImplementedException();
    }

    private bool CheckCD()
    {
        throw new NotImplementedException();
    }

    private void FollowPlayer()
    {
        Vector3 newPos = player.position + offset;

        transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
    }

    public bool CheckCombatState()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Cambia el valor de CheckCombat
            CheckCombat = !CheckCombat;

        }
        return CheckCombat;
    }


}
