using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIEquipmentManager : MonoBehaviour
{
    public GameObject SlotPrefab;
    public Transform EquipmentItemContainer;
    public Sprite emptySlot;

    private static GameObject This;

    public GameObject EquipmentOwner;
    EquipmentInventory Equipment;

    public GameObject WeaponSlot;
    public GameObject TorsoSlot;
    public GameObject LegSlot;
    public GameObject HandSlot;

    private void Awake()
    {
        Equipment = EquipmentOwner.GetComponent<EquipmentInventory>();
        InitializeCharacterSlots();
        This = this.gameObject;
    }

    private void Update()
    {
        foreach(ItemSlot itemSlot in Equipment.Container)
        {
            if(itemSlot != null)
            {
                GameObject slotToChange = CheckSlot(itemSlot);
                Slot slot = slotToChange.GetComponent<Slot>(); //
                if (slot.itemSlot.Item != null)
                {
                    slot.icon.GetComponent<Image>().sprite = slot.itemSlot.Item.icon;
                }
                else
                {
                    slot.icon.GetComponent<Image>().sprite = emptySlot;
                    slot.text.GetComponent<Text>().text = "";
                }
            }
        }
    }

    GameObject CheckSlot(ItemSlot slot)
    {
        switch (slot)
        {
            case EquipmentSlots.WeaponSlot:
                return WeaponSlot;
            case EquipmentSlots.TorsoSlot:
                return TorsoSlot;
            case EquipmentSlots.LegSlot:
                return LegSlot;
            case EquipmentSlots.HandSlot:
                return HandSlot;
            default:
                return null;
        }
    }
    public static bool TryEquip(Item item, GameObject slot)
    {
        switch (slot.name)
        {
            case "WeaponSlot":
                if(item.type == Item.Type.Weapon)
                {
                    EquipItem(item, slot);
                    return true;
                }
                return false;
            case "TorsoSlot":
                if (item.type == Item.Type.TorsoArmor)
                {
                    EquipItem(item, slot);
                    return true;
                }
                return false;
            case "LegSlot":
                if (item.type == Item.Type.TorsoArmor)
                {
                    EquipItem(item, slot);
                    return true;
                }
                return false;
            case "HandSlot":
                if (item.type == Item.Type.TorsoArmor)
                {
                    EquipItem(item, slot);
                    return true;
                }
                return false;
            default:
                return false;

        }
    }

    private static void EquipItem(Item item, GameObject slot)
    {
        GUIEquipmentManager instance = GameObject.FindObjectOfType<GUIEquipmentManager>();

        switch (slot.name)
        {
            case "WeaponSlot":
                instance.WeaponSlot.GetComponent<Slot>().itemSlot.Item = item;
                instance.WeaponSlot.GetComponent<Slot>().icon.GetComponent<Image>().sprite = item.icon;
                break;
            case "TorsoSlot":
                instance.TorsoSlot.GetComponent<Slot>().itemSlot.Item = item;
                instance.TorsoSlot.GetComponent<Slot>().icon.GetComponent<Image>().sprite = item.icon;
                break;
            case "LegSlot":
                instance.LegSlot.GetComponent<Slot>().itemSlot.Item = item;
                instance.LegSlot.GetComponent<Slot>().icon.GetComponent<Image>().sprite = item.icon;
                break;
            case "HandSlot":
                instance.HandSlot.GetComponent<Slot>().itemSlot.Item = item;
                instance.HandSlot.GetComponent<Slot>().icon.GetComponent<Image>().sprite = item.icon;
                break;
            default:
                break;
        }

        //Equip on char
    }

    private void InitializeCharacterSlots()
    {
        WeaponSlot.GetComponent<Slot>().itemSlot = new ItemSlot();
        TorsoSlot.GetComponent<Slot>().itemSlot = new ItemSlot();
        LegSlot.GetComponent<Slot>().itemSlot = new ItemSlot();
        HandSlot.GetComponent<Slot>().itemSlot = new ItemSlot();
    }

}
