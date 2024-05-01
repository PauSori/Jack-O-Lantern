using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeFinal : MonoBehaviour
{
    public GameObject panelFinal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(panelFinal.activeSelf == false)
        {
            Menu();
        }
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
