using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurarCoop : MonoBehaviour
{
    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            healthSlider.value -= 0.1f;
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("arrow"))
    //    {
    //        Debug.Log("Curado");
    //        healthSlider.value += 0.1f;
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("arrow"))
        {
            Debug.Log("Curado");
            healthSlider.value += 0.1f;
        }
    }
}
