using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject CharacterWindow;
    public GameObject Camera;
    private void Awake()
    {
        Inventory.SetActive(true);
        Inventory.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        Toggle(Inventory, KeyCode.Tab);
        Toggle(CharacterWindow, KeyCode.C);
    }

    void Toggle(GameObject UIElement, KeyCode keyCode)
    {
        if (!UIElement.activeSelf)
        {
            if (Input.GetKeyDown(keyCode))
            {
                UIElement.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                Camera.GetComponent<CameraScript>().enabled = false;
            }
        }
        else if (UIElement.activeSelf)
        {
            if (Input.GetKeyDown(keyCode))
            {
                UIElement.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Camera.GetComponent<CameraScript>().enabled = true;
            }
        }
    }
}
