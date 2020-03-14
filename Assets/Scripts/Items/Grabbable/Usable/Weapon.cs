using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Usable
{
    public float damage = 0f;
    // Start is called before the first frame update
    void Awake()
    {

    }

    new private void Start()
    {
        inventory = GameObject.Find("Inventory");
        slot = equip_slot.weapon;
        type = Type.weapon;
    }
}
