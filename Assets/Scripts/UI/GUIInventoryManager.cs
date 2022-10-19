using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIInventoryManager : MonoBehaviour
{
    public Transform InventoryItemContainer;
    public GameObject SlotPrefab;

    public GameObject InventoryOwner;
    Inventory Inventory;
    public GameObject[] GUIInventory;

    public int InventoryColumns;
    public int InventoryRows;

    public Sprite emptySlot;


    //for dragging items
    GameObject draggedSlot;
    Slot draggedSlotComponent;
    bool dragged;
    Vector3 mousepos;

    void Awake()
    {
        Inventory = InventoryOwner.GetComponent<Inventory>();
        GUIInventory = new GameObject[Inventory.SlotCount];

        for (int j = 0; j < Inventory.SlotCount; j++)
        {
            GameObject obj = Instantiate(SlotPrefab, InventoryItemContainer);

            Slot objslot = obj.GetComponent<Slot>();
            ItemSlot itemSlot = new ItemSlot();
            objslot.itemSlot = itemSlot;

            objslot.itemSlot.Item = null;
            objslot.itemSlot.Quantity = 0;
            objslot.icon.GetComponent<Image>().sprite = emptySlot;
            objslot.text.GetComponent<Text>().text = "";
            objslot.itemSlot.slotIndex = j;

            GUIInventory[j] = obj;
        }
    }
    //spaghetti ahead, you have been warned
    private void Update()
    {
        for(int j = 0; j< Inventory.SlotCount; j++)
        {
            Slot slot = GUIInventory[j].GetComponent<Slot>();
            if (Inventory.Container[j].Item != null)
            {
                slot.itemSlot.Item = Inventory.Container[j].Item;
                slot.itemSlot.Quantity = Inventory.Container[j].Quantity;
                slot.icon.GetComponent<Image>().sprite = Inventory.Container[j].Item.icon;
                slot.text.GetComponent<Text>().text = Inventory.Container[j].Quantity.ToString();
            }
            else
            {
                slot.itemSlot.Item = null;
                slot.itemSlot.Quantity = 0;
                slot.icon.GetComponent<Image>().sprite = emptySlot;
                slot.text.GetComponent<Text>().text = "";
            }
        }

        DragItem();
    }
    public void SpawnItem(Slot slot)
    {
        GameObject spawn = Instantiate(slot.itemSlot.Item.prefab);
        Inventory.RemoveItem(slot.itemSlot.Item, 1, slot.itemSlot.slotIndex);
        GameObject playerorientation = InventoryOwner.transform.GetChild(0).gameObject;
        spawn.transform.position = playerorientation.transform.position + playerorientation.transform.forward * 3f;
    }
    public void EquipItem()
    {

    }
    public void DragItem()
    {
        if (Input.GetMouseButtonUp(0)) //on mouse button release
        {
            List<RaycastResult> raycasthits = RaycastMouse();

            foreach (RaycastResult hit in raycasthits)
            {
                Slot hitSlotComponent = hit.gameObject.GetComponent<Slot>();

                //inventory
                if (hit.gameObject.tag == "Slot")
                {
                    //spawn from inventory
                    if (!dragged)
                    {
                        if (hitSlotComponent.itemSlot.Item != null)
                        {
                            SpawnItem(hitSlotComponent);
                            draggedSlot = null;
                            draggedSlotComponent = null;
                            dragged = false;
                        }
                    }
                    //swap places inside inventory
                    else if ((hitSlotComponent.itemSlot.Item != null) && (hitSlotComponent.itemSlot.Item != draggedSlotComponent.itemSlot.Item))
                    {
                        Item Item = hitSlotComponent.itemSlot.Item;
                        int Quantity = hitSlotComponent.itemSlot.Quantity;

                        Inventory.RemoveItem(hitSlotComponent.itemSlot.Item, hitSlotComponent.itemSlot.Quantity, hitSlotComponent.itemSlot.slotIndex);

                        Inventory.AddItem(draggedSlotComponent.itemSlot.Item, draggedSlotComponent.itemSlot.Quantity, hitSlotComponent.itemSlot.slotIndex);
                        Inventory.AddItem(Item, Quantity, draggedSlotComponent.itemSlot.slotIndex);

                        Destroy(draggedSlot);
                        draggedSlot = null;
                        draggedSlotComponent = null;
                        dragged = false;
                    }
                    //drag to another slot inside inventory
                    else if(dragged)
                    {
                        Inventory.AddItem(draggedSlotComponent.itemSlot.Item, draggedSlotComponent.itemSlot.Quantity, hitSlotComponent.itemSlot.slotIndex);
                        Destroy(draggedSlot);
                        draggedSlot = null;
                        draggedSlotComponent = null;
                        dragged = false;
                    }
                }
                //equipment slot
                else if (hit.gameObject.tag == "EquipmentSlot")
                {
                    //drag to another slot inside inventory
                    if (dragged)
                    {
                        if (GUIEquipmentManager.TryEquip(draggedSlotComponent.itemSlot.Item, hit.gameObject))
                        {
                            Destroy(draggedSlot);
                            draggedSlot = null;
                            draggedSlotComponent = null;
                            dragged = false;
                        }
                        else
                        {
                            Inventory.AddItem(draggedSlotComponent.itemSlot.Item, draggedSlotComponent.itemSlot.Quantity);
                            Destroy(draggedSlot);
                            draggedSlot = null;
                            draggedSlotComponent = null;
                            dragged = false;
                        }
                    }
                }
                //prevent from spawning it from between slots
                else if (hit.gameObject.tag == "Inventory" && dragged)
                {
                    Inventory.AddItem(draggedSlotComponent.itemSlot.Item, draggedSlotComponent.itemSlot.Quantity);
                    Destroy(draggedSlot);
                    draggedSlot = null;
                    draggedSlotComponent = null;
                    dragged = false;
                }
            }
            //drag out of inventory
            if (dragged || (draggedSlot != null))
            {
                for (int i = 1; i <= draggedSlotComponent.itemSlot.Quantity; i++)
                {
                    SpawnItem(draggedSlotComponent);
                }
                Destroy(draggedSlot);
                draggedSlot = null;
                draggedSlotComponent = null;
                dragged = false;
            }
        }
        //start dragging
        if (Input.GetMouseButton(0))
        {
            //get slot under mouse
            if (draggedSlot == null && !dragged)
            {
                List<RaycastResult> raycasthits = RaycastMouse();

                foreach (RaycastResult hit in raycasthits)
                {
                    if (hit.gameObject.tag == "Slot" || hit.gameObject.tag == "EquipmentSlot")
                    {
                        if (hit.gameObject.GetComponent<Slot>().itemSlot.Item != null)
                        {
                            //to get only 1 item from the slot
                            if (Input.GetKey(KeyCode.LeftShift))
                            {
                                draggedSlot = Instantiate(hit.gameObject);
                                draggedSlotComponent = draggedSlot.GetComponent<Slot>();
                                draggedSlotComponent.itemSlot = (ItemSlot)hit.gameObject.GetComponent<Slot>().itemSlot.Clone();

                                //draggedSlotComponent.itemSlot = (ItemSlot)hit.gameObject.GetComponent<Slot>().itemSlot.Clone();
                                draggedSlotComponent.itemSlot.Quantity = 1;
                                draggedSlotComponent.text.GetComponent<Text>().text = draggedSlotComponent.itemSlot.Quantity.ToString();
                                draggedSlot.tag = "Untagged";
                            }
                            else
                            {
                                draggedSlot = Instantiate(hit.gameObject);
                                draggedSlotComponent = draggedSlot.GetComponent<Slot>();
                                draggedSlotComponent.itemSlot = (ItemSlot)hit.gameObject.GetComponent<Slot>().itemSlot.Clone();

                                draggedSlot.tag = "Untagged";
                            }
                            draggedSlot.SetActive(false);
                            mousepos = Input.mousePosition;
                        }
                    }
                }
            }
            //detect if dragging should start
            else if (draggedSlot != null && !dragged)
            {
                if (Vector3.Distance(mousepos, Input.mousePosition) > 10)
                {
                    draggedSlot.transform.SetParent(gameObject.transform, false);
                    draggedSlot.SetActive(true);
                    Inventory.RemoveItem(draggedSlotComponent.itemSlot.Item, draggedSlotComponent.itemSlot.Quantity, draggedSlotComponent.itemSlot.slotIndex);
                    dragged = true;
                }
            }
            //start moving the item
            else if (dragged)
            {
                draggedSlot.transform.position = Input.mousePosition;
            }
        }
    }
    
    public List<RaycastResult> RaycastMouse()
    {

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);


        return results;
    }
}
