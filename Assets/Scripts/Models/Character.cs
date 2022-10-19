using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new item", menuName = "Character/Create new character")]
public class Character : ScriptableObject
{
    public int id;
    public new string name;
    public GameObject prefab;

    public Slot WeaponSlot;
}
