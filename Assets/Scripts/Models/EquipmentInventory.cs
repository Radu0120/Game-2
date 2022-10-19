using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory : MonoBehaviour
{
    public List<ItemSlot> Container;

    public EquipmentSlots.WeaponSlot WeaponSlot;
    public EquipmentSlots.TorsoSlot TorsoSlot;
    public EquipmentSlots.LegSlot LegSlot;
    public EquipmentSlots.HandSlot HandSlot;

    // Start is called before the first frame update
    void Awake()
    {
        WeaponSlot = new EquipmentSlots.WeaponSlot();
        HandSlot = new EquipmentSlots.HandSlot();
        TorsoSlot = new EquipmentSlots.TorsoSlot();
        LegSlot = new EquipmentSlots.LegSlot();

        Container = new List<ItemSlot>() { WeaponSlot, TorsoSlot, LegSlot, HandSlot };
    }

    public void Equip(Item item, ItemSlot itemSlot)
    {
        switch (item.type)
        {
            //case Item.Type.Weapon:

        }
    }
}
