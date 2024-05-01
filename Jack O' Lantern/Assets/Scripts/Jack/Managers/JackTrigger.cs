using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JackTrigger : MonoBehaviour
{
    EventSystem m_EventSystem;
    public GameObject winbutton;

    public GameObject final;
    public GameObject jack;
    public GameObject tombs;
    // Start is called before the first frame update
    void OnEnable()
    {
        //Fetch the current EventSystem. Make sure your Scene has one.
        m_EventSystem = EventSystem.current;
    }
    void Start()
    {
        jack.SetActive(false);
        tombs.SetActive(false);
        final.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (jack == null)
        {
            final.SetActive (true);
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            m_EventSystem.SetSelectedGameObject(winbutton);
            jack.SetActive(true);
            tombs.SetActive(true);
        }
    }
}
