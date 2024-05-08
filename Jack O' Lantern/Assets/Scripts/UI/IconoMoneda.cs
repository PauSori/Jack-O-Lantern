using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconoMoneda : MonoBehaviour
{
    public GameObject moneda;
    public GameObject iconoMoneda;

    public GameObject manzana;
    public GameObject iconoManzana;
    // Start is called before the first frame update
    void Start()
    {
        iconoMoneda.SetActive(false);
        iconoManzana.SetActive(false);    
    }

    // Update is called once per frame
    void Update()
    {
        if(moneda == null)
        {
            iconoMoneda.SetActive (true);
        }
        if(manzana == null)
        {
            iconoManzana.SetActive (true);
        }
    }
}
