using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaController : MonoBehaviour
{
    private bool cameraDisabled = false;

    public Camera Camera;
    public static PuertaController instance;
    private Animator animator;
    public List<GameObject> enemies = new List<GameObject>();
    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Camera.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraDisabled && enemies.Count <= 0)
        {
            Camera.gameObject.SetActive(true);
            animator.SetBool("Open", true);
            Invoke("CameraOff", 5f);
        }

    }
    public void CameraOff()
    {
        Time.timeScale = 1f;
        Camera.gameObject.SetActive(false);
        cameraDisabled = true;


    }
    public void RemoveGameObject(GameObject obj)
    {
        enemies.Remove(obj);
    }

    // Llama a la coroutine cuando sea necesario

}
