using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public float maxDistance = 10f;

    public Transform Camera;

    public LayerMask layerMask;

    GameObject grabbedObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hitInfo, maxDistance, layerMask) && Input.GetMouseButtonDown(0))
        {
            grabbedObject = hitInfo.collider.gameObject;
            grabbedObject.GetComponent<Rigidbody>().useGravity = false;
        }
        
    }

    private void FixedUpdate()
    {
        if (grabbedObject != null)
            MoveToPosition(grabbedObject);
    }

    void MoveToPosition(GameObject gameObject)
    {
        Vector3 pos = Camera.transform.position + Camera.transform.forward * 3f;

        Vector3 toCameraPoint = pos - gameObject.transform.position;

        //gameObject.transform.position =  Vector3.Lerp(gameObject.transform.position, pos, Time.deltaTime * 20);

        gameObject.GetComponent<Rigidbody>().AddForce(toCameraPoint, ForceMode.VelocityChange);

        gameObject.GetComponent<Rigidbody>().velocity *= Vector3.Distance(gameObject.transform.position, pos);

        if(gameObject.GetComponent<Rigidbody>().velocity.magnitude > 50f)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        } 

        //gameObject.GetComponent<Rigidbody>().AddForce(toCameraPoint * 50, ForceMode.Impulse);
    }
}
