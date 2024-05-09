using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressController : MonoBehaviour
{
    private GameObject moneda;
    private GameObject manzana;
    // Start is called before the first frame update
    void Start()
    {
        moneda = GameObject.Find("moneda");
        manzana = GameObject.Find("manzana");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.K))
        {
            Destroy(moneda);
        }
        if(Input.GetKeyUp(KeyCode.L))
        {
            Destroy (manzana);
        }
    }
}
