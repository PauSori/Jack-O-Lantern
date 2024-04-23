using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxCinematic : MonoBehaviour
{
    public GameObject Dialogue;
    public GameObject Dialogue2;
    public GameObject Dialogue3;
    public GameObject booleana;
    public GameObject booleana2;
    public GameObject SelectPlayer;

    public GameObject CanvasSelectPlayer;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (booleana.activeSelf == false)
        {
            animator.SetBool("Dialogue2", true);
        }
        if (booleana2.activeSelf == false)
        {
            animator.SetBool("Dialogue2", false);
            animator.SetBool("Dialogue3", true);

        }
        if (SelectPlayer.activeSelf == false)
        {
            animator.SetBool("Dialogue3", false);
            animator.SetBool("Select", true);

        }
    }
    public void SetActive()
    {
        Dialogue.SetActive(true);
    }
    public void SetActive2()
    {
        Dialogue2.SetActive(true);
    }
    public void SetActive3()
    {
        Dialogue3.SetActive(true);
    }
    public void selectPlayer()
    {
        CanvasSelectPlayer.SetActive(true);
    }
}
