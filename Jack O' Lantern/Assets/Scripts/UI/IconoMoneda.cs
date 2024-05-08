using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconoMoneda : MonoBehaviour
{
    public GameObject moneda;
    public GameObject iconoMoneda;
    // Start is called before the first frame update
    void Start()
    {
        iconoMoneda.SetActive(false);    
    }

    // Update is called once per frame
    void Update()
    {
        if(moneda == null)
        {
            iconoMoneda.SetActive (true);
        }
    }
}
