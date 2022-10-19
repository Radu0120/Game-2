using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPickUpScript : MonoBehaviour
{
    public Transform Camera;
    public LayerMask layerMask;
    Text Text;
    // Update is called once per frame
    private void Start()
    {
        Text = GetComponent<Text>();
    }
    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.position, Camera.TransformDirection(Vector3.forward), out hitInfo, 3,layerMask))
        {
            ItemController component;
            if (hitInfo.collider.gameObject.TryGetComponent(out component))
            {
                Text.text = $"Pick up {component.Item.name}";
            }
        }
        else
        {
            Text.text = "";
        }
    }
}
