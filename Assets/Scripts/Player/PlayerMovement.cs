using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed;

    public float groundDrag;

    [Header("Ground check")]
    public float playerHeight;
    public LayerMask groundLayer;
    bool grounded;

    public float jumpForce;
    public float jumpCooldown;
    public bool readyToJump;
    public float airMultiplier;


    public float horizontalInput;
    public float verticalInput;

    public Transform orientation;

    Vector3 MoveDirection;

    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        MyInput();
        LimitVelocity();
        

        if (grounded) rb.drag = groundDrag;
        else rb.drag = 0;

    }

    void FixedUpdate()
    {
        MovePlayer();
        readyToJump = true;
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && grounded && readyToJump)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    void MovePlayer()
    {
        MoveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
            rb.AddForce(MoveDirection.normalized * MoveSpeed * 10, ForceMode.Force);
        else
            rb.AddForce(MoveDirection.normalized * MoveSpeed * 10 * airMultiplier, ForceMode.Force);
    }
    private void LimitVelocity()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatvel.magnitude > MoveSpeed)
        {
            Vector3 limitvel = flatvel.normalized * MoveSpeed;
            rb.velocity = new Vector3(limitvel.x, rb.velocity.y, limitvel.z);
        }
    }
    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}
