using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : Grabbable
{
    public bool equiped;
    protected enum equip_slot
    {
        armor, weapon, consumable
    }
    protected equip_slot slot;
    new protected void Start()
    {
        inventory = GameObject.Find("Inventory");
    }
}
