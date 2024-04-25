using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFlaco : MonoBehaviour
{
    public GameObject luz;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(luz.activeSelf)
        {
            animator.SetBool("Select", true);
        }
        else if(luz.activeSelf == false)
        {
            animator.SetBool("Select", false);
        }
    }
}
