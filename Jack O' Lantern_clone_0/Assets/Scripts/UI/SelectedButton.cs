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
        // Obtén el EventSystem actual. Asegúrate de que tu escena tenga uno.
        m_EventSystem = EventSystem.current;
    }

    private void Start()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // El puntero ha entrado en el botón
        // Realiza acciones específicas aquí
        Debug.Log("El puntero está sobre el botón.");
        m_EventSystem.SetSelectedGameObject(gameObject);
        // Verifica si el botón está seleccionado
        if (gameObject != m_EventSystem.currentSelectedGameObject)
        {
            luz.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // El puntero ha salido del botón
        // Revierte las acciones realizadas en OnPointerEnter
        Debug.Log("El puntero ha salido del botón.");
        // Desactiva la luz
        luz.SetActive(false);
    }

    private void Update()
    {
        // Activa la luz solo si el botón está seleccionado
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
