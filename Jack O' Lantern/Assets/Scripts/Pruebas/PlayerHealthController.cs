using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    //CON MANDO
    EventSystem m_EventSystem;
    public GameObject reinciarButton;

    public bool vivo = true;
    public Slider healthSlider;
    public GameObject hitPlayerDamage;
    public GameObject gameOver;
    public GameObject curado;
    //public bool flashHealth = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        //Fetch the current EventSystem. Make sure your Scene has one.
        m_EventSystem = EventSystem.current;
    }
    void Start()
    {
        //healthSlider= GameObject.Find("PlayerHealth").GetComponent<Slider>();


        hitPlayerDamage.SetActive(false);
        curado.SetActive(false);
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(healthSlider.value <= 0 && vivo == true)
        {
            GameOver();
        }
        if (curado.activeSelf)
        {
            Invoke("curarse", 1.5f);
        }
    }
    public void curarse()
    {
        curado.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            healthSlider.value -= 0.1f;
            hitPlayerDamage.SetActive(true);
            Invoke("hitPlayer", 0.5f);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            healthSlider.value -= 0.1f;
            hitPlayerDamage.SetActive(true);
            Invoke("hitPlayer", 0.5f);
        }
    }
    public void hitPlayer()
    {
        hitPlayerDamage.SetActive(false);

    }
    public void GameOver()
    {
        vivo = false;
        m_EventSystem.SetSelectedGameObject(reinciarButton);
        Cursor.lockState = CursorLockMode.None;
        gameOver.SetActive(true);
        Time.timeScale = 0f;
        
    }
}
