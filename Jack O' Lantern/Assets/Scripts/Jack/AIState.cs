using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : MonoBehaviour
{
    public float maxHP = 100f;
    float currentHP;

    public void Start()
    {
        currentHP = maxHP;
    }

    private void Update()
    {
        CheckStates();

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentHP -= 10;
            if (currentHP < 0)
            {
                currentHP = 0;
            }
        }
    }

    public void CheckStates()
    {
        if (currentHP <= maxHP / 2)
        {
            HalfLifeState();
        }


    }

    void HalfLifeState()
    {
        CheckForTombs();
    }

    void CheckForTombs()
    {
        int tombCount = CountTombs();

        if (tombCount > 0)
        {
            Debug.Log("Sart TP State");
            //Aqui viene la lógica de el estado de TP

        }
        else
        {
            CheckSummonRange();
        }
    }
    int CountTombs()
    {
        // Busca todos los objetos en la escena con la etiqueta "Tomb"
        GameObject[] tombs = GameObject.FindGameObjectsWithTag("Tomb");

        // Devuelve el número de tumbas
        return tombs.Length;
    }

    void CheckSummonRange()
    {

    }

}
