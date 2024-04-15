using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        healthSlider= GameObject.Find("HealthPlayer").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
           
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            healthSlider.value -= 0.1f;
        }

    }
}
