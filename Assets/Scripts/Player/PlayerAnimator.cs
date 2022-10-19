using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Transform cameraPos;
    public float cameraDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!transform.parent.GetComponent<PlayerMovement>().readyToJump)
        {
            transform.GetComponent<Animator>().SetTrigger("Jump");
        }
        if (transform.parent.GetComponent<PlayerMovement>().verticalInput > 0)
        {
            transform.GetComponent<Animator>().SetBool("MovingForward", true);
        }
        else
        {
            transform.GetComponent<Animator>().SetBool("MovingForward", false);
        }
        if (transform.parent.GetComponent<PlayerMovement>().verticalInput < 0)
        {
            transform.GetComponent<Animator>().SetBool("MovingBackward", true);
        }
        else
        {
            transform.GetComponent<Animator>().SetBool("MovingBackward", false);
        }
        if (transform.parent.GetComponent<PlayerMovement>().horizontalInput < 0)
        {
            transform.GetComponent<Animator>().SetBool("MovingRight", true);
        }
        else
        {
            transform.GetComponent<Animator>().SetBool("MovingRight", false);
        }
        if (transform.parent.GetComponent<PlayerMovement>().horizontalInput > 0)
        {
            transform.GetComponent<Animator>().SetBool("MovingLeft", true);
        }
        else
        {
            transform.GetComponent<Animator>().SetBool("MovingLeft", false);
        }
    }
}
