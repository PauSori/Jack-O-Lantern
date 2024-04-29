using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombs : MonoBehaviour
{
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
    }

    private void OnDestroy()
    {
        AIState.instance.RemoveGameObject(gameObject);
    }
}
