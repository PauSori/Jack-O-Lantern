using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tombs : MonoBehaviour
{
    public Slider tombs;

    private void Start()
    {
        tombs = GetComponentInChildren<Slider>(); 
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDestroy();
            Destroy(gameObject);
        }

        if(this.gameObject == null)
        {
            OnDestroy();
        }
        if(tombs.value <= 0)
        {
            OnDestroy();

        }
    }

    private void OnDestroy()
    {
        AIState.instance.RemoveGameObject(gameObject);
        Destroy(gameObject);
    }

}
