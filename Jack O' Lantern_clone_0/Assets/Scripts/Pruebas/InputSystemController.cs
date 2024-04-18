using Org.BouncyCastle.Asn1.Cmp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HandAttack(InputAction.CallbackContext context)
    {
        print(context.phase);

        if (context.performed)
        {
            print("Attack performed");
        }
        else if(context.started)
        {
            print("Attack started");
        }
        else if(context.canceled)
        {
            print("Attack canceled");
        }
    }
}
