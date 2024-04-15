using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PuertaGrandeController : MonoBehaviour
{
    private Animator animator;

    public GameObject moneda;
    public GameObject panel;
    public GameObject panelMoneda;
    public bool monedaPillada;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        panelMoneda.SetActive(false);
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (moneda == null)
        {
            monedaPillada = true;
        }
        else if(moneda !=  null)
        {
            monedaPillada = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !monedaPillada)
        {
            panel.SetActive(true);
        }
        if (other.CompareTag("Player") && monedaPillada)
        {
            panelMoneda.SetActive(true);
            animator.SetBool("Open", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        panel.SetActive(false);
        panelMoneda.SetActive(false);
    }
}
