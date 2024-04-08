using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class JSONManager : MonoBehaviour
{
    public static GameManager manager;


    // Start is called before the first frame update
    void Start()
    {
        manager = new GameManager();
        string jsondata = JsonUtility.ToJson(manager);
        string path = Application.persistentDataPath + "/savedata.json";
        File.WriteAllText(path, jsondata);
        LoadGame();
    }
    void LoadGame()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if (File.Exists(path))
        {
            string jsondata = File.ReadAllText(path);
            manager = new GameManager();
            manager= JsonUtility.FromJson<GameManager>(jsondata);

            Debug.Log(jsondata);
        }
    }
}
