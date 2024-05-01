using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Mapa()
    {
        SceneManager.LoadScene("PruebaMapa");
    }
    public void Cinmetica()
    {
        SceneManager.LoadScene("Cinematica");
    }
    public void exitGame()
    {
        Debug.Log("se ha salido");
        Application.Quit();
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
