using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveData
{
    public int lives;
    public float posX;
    public float posY;
    public float posZ;
    public string sceneName;
}

public class ZonaGuardarController : MonoBehaviour
{
    string filePath;
    SaveData saveData;
    //private static readonly string SAVE_FILE = "/player.json";
    public Transform playerTransform;

    public float rangoGuardar = 5f;
    //void Awake()
    //{
    //    DontDestroyOnLoad(this.gameObject);
    //}
    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        saveData = new SaveData(); // Agrega esta línea
        LoadGame();
    }
    private void Update()
    {
        if(Vector3.Distance(transform.position, playerTransform.position) < rangoGuardar)
        {
            SavePlayerPosition(playerTransform.position);
        }
        if(Input.GetKeyUp(KeyCode.G))
        {
            SavePlayerPosition(playerTransform.position);
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            LoadGame();
        }
    }
    public void SavePlayerPosition(Vector3 playerPosition)
    {
        //SaveData save = new SaveData();
        saveData.posX = playerPosition.x;
        saveData.posY = playerPosition.y;
        saveData.posZ = playerPosition.z;
        saveData.sceneName = SceneManager.GetActiveScene().name;

        string jsonString = JsonUtility.ToJson(saveData);
        filePath = Application.persistentDataPath + "/PlayerData.json";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        File.WriteAllText(filePath, jsonString);
        Debug.Log($"Player position saved to {filePath}");
    }

    public Vector3 LoadPlayerPosition()
    {
        filePath = Application.persistentDataPath + "/PlayerData.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData save = JsonUtility.FromJson<SaveData>(json);
            return new Vector3(save.posX, save.posY, save.posZ);
        }
        else
        {
            Debug.LogWarning("No save file found. Using default position.");
            return Vector3.zero;
        }
    }
    public void LoadGame()
    {
        if (File.Exists(filePath))
        {
            string loadPlayerData = File.ReadAllText(filePath);
            saveData = JsonUtility.FromJson<SaveData>(loadPlayerData);

            // Actualiza la posición del jugador con los valores cargados
            playerTransform.position = new Vector3(saveData.posX, saveData.posY, saveData.posZ);

            Debug.Log($"Loaded player data: X={saveData.posX}, Y={saveData.posY}, Z={saveData.posZ}");
        }
        else
        {
            Debug.LogWarning("No save file found. Using default position.");
        }
    }
    public void DeleteSaveFile()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);

            Debug.Log("Save file deleted!");
        }
        else
            Debug.Log("There is nothing to delete!");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoGuardar);
    }
}
