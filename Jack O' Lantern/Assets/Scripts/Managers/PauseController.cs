using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseController : MonoBehaviour
{    
    //PAUSA CON MANDO
    EventSystem m_EventSystem;
    public GameObject pauseButton;

    public GameObject pause;
    public bool pausaActiva = false;
    void OnEnable()
    {
        //Fetch the current EventSystem. Make sure your Scene has one.
        m_EventSystem = EventSystem.current;
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        pause.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            Pausa();
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7)) && pausaActiva == true)
        {

            Resume();
        }
    }

    public void Pausa()
    {
        m_EventSystem.SetSelectedGameObject(pauseButton);
        Cursor.lockState = CursorLockMode.None;
        pause.SetActive(true);
        pausaActiva = true;
        Time.timeScale = 0.0f;
    }
    public void Resume()
    {
        pausaActiva = false;
        Time.timeScale = 1.0f;
        Debug.Log("despausa");
        Cursor.lockState = CursorLockMode.Locked;
        pause.SetActive(false);

    }
}
