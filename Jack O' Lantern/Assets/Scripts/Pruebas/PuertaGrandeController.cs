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

    public GameObject monedaObtenida;

    public CapsuleCollider capsuleCollider;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        panelMoneda.SetActive(false);
        animator = GetComponent<Animator>();
        monedaObtenida.SetActive(false);
        capsuleCollider = GetComponent<CapsuleCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moneda == null && monedaPillada == false)
        {
            monedaPillada = true;
            monedaObtenida.SetActive (true);
        }
        else if(moneda !=  null)
        {
            monedaPillada = false;
        }
        if(monedaObtenida.activeSelf)
        {
            Invoke("MonedaObtenida", 3.0f);
        }
        if (panelMoneda.activeSelf)
        {
            Invoke("PuedesPasar", 3.0f);
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
            capsuleCollider.enabled = false;

        }
    }
    public void PuedesPasar()
    {
        panelMoneda.SetActive(false);
    }
    public void MonedaObtenida()
    {
        monedaObtenida.SetActive(false);
    }
    private void OnTriggerExit(Collider other)
    {
        panel.SetActive(false);
        panelMoneda.SetActive(false);
    }
}
