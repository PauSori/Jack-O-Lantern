using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public Slider healthSlider;
    public GameObject hitPlayerDamage;
    public GameObject gameOver;
    public GameObject curado;
    // Start is called before the first frame update
    void Start()
    {
        healthSlider= GameObject.Find("HealthPlayer").GetComponent<Slider>();
        hitPlayerDamage.SetActive(false);
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(healthSlider.value <= 0)
        {
            GameOver();
        }
        if (curado == true)
        {
            Invoke("curarse", 6.5f);
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
    public void hitPlayer()
    {
        hitPlayerDamage.SetActive(false);

    }
    public void GameOver()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0f;
    }
}
