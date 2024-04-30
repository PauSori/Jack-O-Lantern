using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackTrigger : MonoBehaviour
{
    public GameObject jack;
    public GameObject tombs;
    // Start is called before the first frame update
    void Start()
    {
        jack.SetActive(false);
        tombs.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            jack.SetActive(true);
            tombs.SetActive(true);
        }
    }
}
