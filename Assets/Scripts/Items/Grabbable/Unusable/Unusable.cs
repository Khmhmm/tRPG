using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unusable : Grabbable
{
    public int quantity=1;
    bool contains = false;
    int listIndex = 0;
    private void Awake()
    {
        type = Type.other;
    }

    new void FixedUpdate()
    {
        if (playerIsHere && Input.GetKey(KeyCode.E))
        {
            var inventoryScript = inventory.GetComponent<Inventory>();
            foreach (var item in inventoryScript.items_here)
            {
                var unusScript = item.GetComponent<Unusable>();
                if (unusScript != null)
                {
                    if (unusScript.item_name == this.item_name)
                    {
                        contains = true;
                        listIndex = inventoryScript.items_here.IndexOf(item);
                    }
                }
            }
            inventory.GetComponent<Inventory>().inventory_is_opened = false;
            if (contains)
            {
                inventoryScript.items_here[listIndex].GetComponent<Unusable>().quantity += this.quantity;
                Destroy(this.gameObject);
            }
            else
            {
                inventory.GetComponent<Inventory>().items_here.Add(this.gameObject);
                transform.parent = inventory.transform;
                this.gameObject.SetActive(false);
            }
        }
    }
}
