using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LuzCinematicaController : MonoBehaviour
{
    EventSystem m_EventSystem;
    public GameObject luz;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_EventSystem.alreadySelecting)
        {
            luz.SetActive(true);
        }
        else
        {
            luz.SetActive(false);
        }
    }
}
