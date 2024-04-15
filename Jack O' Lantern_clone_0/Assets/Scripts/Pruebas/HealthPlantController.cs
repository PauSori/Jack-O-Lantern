using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlantController : MonoBehaviour
{
    public float healthRange = 5f;
    public Slider sliderPlant;
    public Slider playerHealth;
    private Transform player;
    public GameObject curado;

    // Start is called before the first frame update
    void Start()
    {
        sliderPlant = GetComponentInChildren<Slider>();
        playerHealth = GameObject.Find("HealthPlayer").GetComponent<Slider>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        curado.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (sliderPlant.value <= 0 && Vector3.Distance(transform.position, player.position) < healthRange)
        {
            playerHealth.value = 1f;
            curado.SetActive(true);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, healthRange);
    }
}
