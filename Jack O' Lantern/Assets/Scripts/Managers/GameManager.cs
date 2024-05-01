using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<GameObject> tombs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            tombs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Tomb"));
        }
        else
        {
            Destroy(gameObject);
        }

        // Aquí puedes poner el resto de tu código de inicialización
    }

    public void RemoveTomb(GameObject tomb)
    {
        tombs.Remove(tomb);
    }


}
