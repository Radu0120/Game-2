using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    [Header("Grab settings")]
    public float maxDistance = 3f;
    public float moveForce = 10f;
    public float dropDistance;
    float maxSpeed;
    public float maxForce;
    public float maxCarryWeight;

    [Header("References")]
    public GameObject Player;
    PlayerMovement PlayerMovement;
    Inventory Inventory;
    public Transform Camera;
    public Transform holdParent;
    public LayerMask layerMask;
    GameObject grabbedObject;
    Rigidbody rb;

    [Header("Timer")]
    float counter = 0;
    public float timer = 0.3f;

    private void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
        Inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            counter += Time.deltaTime;
            if (counter >= timer && grabbedObject == null)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(Camera.transform.position, Camera.transform.TransformDirection(Vector3.forward), out hitInfo, maxDistance, layerMask))
                {
                    rb = hitInfo.collider.gameObject.GetComponent<Rigidbody>();
                    GrabObject(hitInfo.collider.gameObject);
                }
                counter = 0;
            }
            else if(grabbedObject != null && Input.GetKeyDown(KeyCode.E))
            {
                DropObject(grabbedObject);
            }
        }
        else if (Input.GetKeyUp(KeyCode.E) && grabbedObject == null)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.transform.position, Camera.transform.TransformDirection(Vector3.forward), out hitInfo, maxDistance, layerMask))
            {
                ItemController component;
                if (hitInfo.collider.gameObject.TryGetComponent(out component))
                {
                    PickUpItem(hitInfo.collider.gameObject);
                }
            }
        }
        else if (counter > 0)
        {
            counter = 0;
        }
    }

    private void FixedUpdate()
    {
        if (grabbedObject != null)
            MoveToPosition(grabbedObject);
    }

    void MoveToPosition(GameObject grabbedObject)
    {
        float distance = Vector3.Distance(holdParent.position, grabbedObject.transform.position);
        //Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
        if (distance > 0.1f)
        {
            if(distance > dropDistance)
            {
                DropObject(grabbedObject);
            }
            
            Vector3 toHoldParent = holdParent.position - grabbedObject.transform.position;
            float playerSpeed = PlayerMovement.MoveSpeed;

            float force = moveForce * playerSpeed * rb.mass * 2 * distance;

            if (force > maxForce)
            {
                rb.position = Vector3.Lerp(rb.position, holdParent.position, Time.deltaTime * 10);
            }
            else
            {
                Vector3 forceVector = toHoldParent * force;
                rb.AddForce(forceVector, ForceMode.Impulse);
            }

            Vector3 fromPlayerToHoldParent = gameObject.transform.position - holdParent.position;
            Vector3 fromPlayerToObject = gameObject.transform.position - rb.position;

            if (rb.velocity.magnitude >= maxSpeed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }

            float angle = Vector3.SignedAngle(fromPlayerToHoldParent, fromPlayerToObject, fromPlayerToHoldParent);
            if(Math.Abs(angle) > 70)
            {
                DropObject(grabbedObject);
            }

            //move by velocity only
            //float speed = Mathf.SmoothStep(minSpeed, maxSpeed, distance/maxDistance) * Time.fixedDeltaTime * 200;
            //rb.velocity = toHoldParent.normalized * speed;
            //Debug.Log(speed);

        }
        //rb.rotation = Quaternion.Lerp(rb.rotation, holdParent.rotation, Time.deltaTime * 70);
        rb.angularVelocity = Vector3.zero;
        if(Math.Abs(Vector3.SignedAngle(rb.rotation.eulerAngles, holdParent.rotation.eulerAngles, Camera.rotation.eulerAngles)) > 0.1f)
        {
            rb.rotation = holdParent.rotation;
        }
    }

    void GrabObject(GameObject grabbedobject)
    {
        //Rigidbody rb = grabbedobject.GetComponent<Rigidbody>();
        holdParent.rotation = rb.rotation;
        if (rb.mass > maxCarryWeight)
        {
            return;
        }
        //Debug.Log(grabbedobject.GetComponent<Renderer>().bounds.size.magnitude);
        if(grabbedobject.GetComponent<Renderer>().bounds.size.magnitude > 3)
        {
            holdParent.localPosition = new Vector3(holdParent.localPosition.x, holdParent.localPosition.y, holdParent.localPosition.z+0.5f);
        }
        rb.drag = 20;
        float playermovespeed = PlayerMovement.MoveSpeed;
        PlayerMovement.MoveSpeed = playermovespeed - playermovespeed * (float)(rb.mass / maxCarryWeight);
        maxSpeed = PlayerMovement.MoveSpeed;
        grabbedObject = grabbedobject;
    }
    void DropObject(GameObject grabbedobject) //for dropping grabbed object
    {
        //Rigidbody rb = grabbedobject.GetComponent<Rigidbody>();
        rb.drag = 1;
        rb.velocity = rb.velocity - rb.velocity * (float)(rb.mass / maxCarryWeight);
        PlayerMovement.MoveSpeed = 9;
        holdParent.localPosition = new Vector3(0, 0, 2);
        holdParent.rotation = holdParent.parent.rotation;
        rb.angularVelocity = Vector3.zero;
        grabbedObject = null;
    }
    void PickUpItem(GameObject item) //pickup item in inventory
    {
        Inventory.AddItem(item.GetComponent<ItemController>().Item, 1);
        Destroy(item);
    }
}
