using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public ItemSlot[] Container;
    public int SlotCount;
    public int StackSize;

    private void Awake()
    {
        Container = new ItemSlot[SlotCount];
        for (int i = 0; i < SlotCount; i++)
        {
            Container[i] = new ItemSlot();
        }
    }
    public void AddItem(Item item, int quantity)
    {
        for (int j = 0; j < SlotCount; j++)
        {
            if (Container[j].Item == item)
            {
                Container[j].Quantity += quantity;
                
                return;
            }
        }
        for (int j = 0; j < SlotCount; j++)
        {
            if (Container[j].Item == null)
            {
                Container[j].Item = item;
                Container[j].Quantity += quantity;

                return;
            }
        }
    }
    //will try to add in the specified slot first, if not possible will try to add it regularly
    public void AddItem(Item item, int quantity, int slot)
    {

        if (Container[slot].Item == null)
        {
            Container[slot].Item = item;

            Container[slot].Quantity += quantity;

            
            
            return;
        }
        else if (Container[slot].Item == item)
        {
            Container[slot].Quantity += quantity;
            
            return;
        }

        for (int j = 0; j < SlotCount; j++)
        {
            if (Container[j].Item == null)
            {
                Container[j].Item = item;
                Container[j].Quantity += quantity;

                
                
                return;
            }
            else if (Container[j].Item == item)
            {
                Container[j].Quantity += quantity;
                
                return;
            }
        }
    }
    public void RemoveItem(Item item, int quantity)
    {
        for (int j = 0; j < SlotCount; j++)
        {
            if (Container[j].Item == item)
            {
                Container[j].Quantity -= quantity;
                

                if (Container[j].Quantity <= 0)
                {
                    Container[j].Item = null;
                    Container[j].Quantity = 0;
                }
                return;

            }
        }
    }
    public void RemoveItem(Item item, int quantity, int slot)
    {

        if (Container[slot].Item == item)
        {
            Container[slot].Quantity -= quantity;
            

            if (Container[slot].Quantity <= 0)
            {
                Container[slot].Item = null;
                Container[slot].Quantity = 0;
            }
            return;

        }
    }
}
