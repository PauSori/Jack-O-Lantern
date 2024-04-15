using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopCamera : MonoBehaviour
{
    public bool cameraBool = false;
    public GameObject cameraObject;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Verifica si la cámara está activa.
        if (cameraObject.activeSelf)
        {
            cameraBool = true;
            characterController.enabled = false;
            Invoke("BoolFalse", 5f);
        }
        else if(cameraBool == false)
        {
            characterController.enabled = true;
        }
    }
    public void BoolFalse()
    {
        cameraBool = false;
    }
}
