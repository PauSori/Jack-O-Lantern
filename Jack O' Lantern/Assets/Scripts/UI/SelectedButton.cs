using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject luz;
    EventSystem m_EventSystem;
    void OnEnable()
    {
        // Obt�n el EventSystem actual. Aseg�rate de que tu escena tenga uno.
        m_EventSystem = EventSystem.current;
    }

    private void Start()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // El puntero ha entrado en el bot�n
        // Realiza acciones espec�ficas aqu�
        Debug.Log("El puntero est� sobre el bot�n.");
        m_EventSystem.SetSelectedGameObject(gameObject);
        // Verifica si el bot�n est� seleccionado
        if (gameObject != m_EventSystem.currentSelectedGameObject)
        {
            luz.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // El puntero ha salido del bot�n
        // Revierte las acciones realizadas en OnPointerEnter
        Debug.Log("El puntero ha salido del bot�n.");
        // Desactiva la luz
        luz.SetActive(false);
    }

    private void Update()
    {
        // Activa la luz solo si el bot�n est� seleccionado
        luz.SetActive(gameObject == m_EventSystem.currentSelectedGameObject);
    }

    public void PointerEnter()
    {
        OnPointerEnter(null);
    }
    public void PointerExit()
    {
        OnPointerExit(null);
    }
}
