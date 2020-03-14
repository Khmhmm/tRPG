using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Usable
{
    public GameObject effect;
    new private void Start()
    {
        inventory = GameObject.Find("Inventory");
        type = Type.consume;
    }
}