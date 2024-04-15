using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class PuertaController : MonoBehaviour
{
    private bool shouldResetEnemies = false;

    private bool cameraDisabled = false;

    public Camera Camera;
    public static PuertaController instance;
    private Animator animator;
    public List<GameObject> enemies = new List<GameObject>();

    private CapsuleCollider capsuleCollider;

    public GameObject enemy;
    public int numberOfEnemiesToSpawn = 5;
    public float spawnRadius = 10f; // Radio de generación
    public Transform spawnPoint; // Punto de origen (por ejemplo, el objeto que representa el suelo)

    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) + spawnPoint.position;
        return spawnPosition;
    }
    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        Camera.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraDisabled && enemies.Count <= 0)
        {
            Camera.gameObject.SetActive(true);
            capsuleCollider.enabled = false;
            animator.SetBool("Open", true);
            Invoke("CameraOff", 5f);
        }
        if (shouldResetEnemies == true)
        {
            ResetEnemies();
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldResetEnemies = true;
            capsuleCollider.enabled = false;
        }
    }

    public void ResetEnemies()
    {
        Debug.Log("Reset de enemigos");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();

        // Genera nuevos enemigos
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition(); // Implementa tu lógica para obtener una posición de generación
            GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
            enemies.Add(newEnemy);
        }

        shouldResetEnemies = false;
    }
    // Llama a la coroutine cuando sea necesario

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPoint.position, spawnRadius);
    }

}
