using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Usable
{
    public enum armor_equip_slot
    {
        head,body,arms,legs
    }
    public armor_equip_slot armor_slot;
    public float resist = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        slot = equip_slot.armor;
        type = Type.armor;
    }
    new private void Start()
    {
        inventory = GameObject.Find("Inventory");
    }
}
