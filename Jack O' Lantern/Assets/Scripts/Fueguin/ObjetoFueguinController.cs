using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoFueguinController : MonoBehaviour
{
    public GameObject panelSeņal;
    public BoxCollider bcollider;
    // Start is called before the first frame update
    void Start()
    {
        bcollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(panelSeņal.activeSelf)
        {
            Invoke("ColliderDesactivar", 5.0f);
        }
    }
    public void ColliderDesactivar()
    {
        bcollider.enabled = false;
    }
}
