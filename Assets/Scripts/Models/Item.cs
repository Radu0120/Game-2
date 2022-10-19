using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new item", menuName ="Item/Create new item")]
public class Item : ScriptableObject
{
    public int id;
    public new string name;
    public Sprite icon;
    public GameObject prefab;
    public Type type;

    public enum Type
    {
        Weapon, HeadArmor, TorsoArmor
    }
}
