using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    //public int slotIndex;
    //public Item Item;
    //public int Quantity;

    public GameObject icon;
    public GameObject text;
    public ItemSlot itemSlot;
}
public class ItemSlot : ICloneable
{
    public int slotIndex;
    public Item Item;
    public int Quantity;

    public object Clone()
    {
        return (ItemSlot)MemberwiseClone();
    }
}
