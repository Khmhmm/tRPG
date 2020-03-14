using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public bool disappearIfEmpty = false;
    GameObject player;
    bool inventorySpawned = false;
    public List<GameObject> containment;
    public List<GameObject> facticialInventory;
    private Inventory plInventoryScript;
    private GUIStyles stylesScript;
    bool isOpened = false;
    // Start is called before the first frame update
    void Start()
    {
        plInventoryScript = GameObject.FindWithTag("Player").GetComponentInChildren<Inventory>();
        stylesScript = GameObject.FindWithTag("MainCamera").GetComponentInChildren<GUIStyles>();
    }

    private void LateUpdate()
    {
        if (!inventorySpawned && containment.Count!=0)
        {
            foreach (var containedItem in containment)
            {
                facticialInventory.Add(Instantiate(containedItem, this.transform));
                containedItem.SetActive(false);
            }
            inventorySpawned = true;
        }

        if (containment.Count == 0 && disappearIfEmpty)
        {
            Destroy(gameObject);
        }
    }

    private void OnGUI()
    {
        GUI.skin = stylesScript.skins[0];
        if (isOpened)
        {
            plInventoryScript.inventory_is_opened = true;
            plInventoryScript.inventory_is_opened = false;
            GUI.Box(new Rect(Screen.width * 0.1f, Screen.height * 0.001f, Screen.width * 0.8f, Screen.height * 0.8f), "");
            if (GUI.Button(new Rect(Screen.width * 0.9f - 20f, Screen.height * 0.001f, 20f, 20f), "", stylesScript.styles[8]))
            {
                isOpened = false;
            }

            GUI.Box(new Rect(Screen.width * 0.26f, Screen.height * 0.001f, 20f, 20f), "", stylesScript.styles[6]);
            GUI.Box(new Rect(Screen.width * 0.86f, Screen.height * 0.001f, 20f, 20f), "", stylesScript.styles[6]);

            // что внутри контейнера
            for (int i = 0; i < facticialInventory.Count; i++)
            {
                var itemScript = facticialInventory[i].GetComponent<Grabbable>();
                var itsUnusable = facticialInventory[i].GetComponent<Unusable>();
                if (itsUnusable != null)
                    GUI.Label(new Rect(Screen.width * 0.86f, Screen.height * 0.1f + i * 20f, 200f, 20f), itemScript.cost.ToString() + "*" + itsUnusable.quantity);
                else
                    GUI.Label(new Rect(Screen.width * 0.86f, Screen.height * 0.1f + i * 20f, 200f, 20f), itemScript.cost.ToString());
                if (GUI.Button(new Rect(Screen.width * 0.7f, Screen.height * 0.1f + i * 20f, Screen.width * 0.15f, 20f), facticialInventory[i].GetComponent<Grabbable>().item_name))
                {
                    var contains = false;
                    var listIndex = 0;
                    foreach (var item in plInventoryScript.items_here)
                    {
                        var unusScript = item.GetComponent<Unusable>();
                        if (unusScript != null)
                        {
                            if (unusScript.item_name == facticialInventory[i].GetComponent<Unusable>().item_name)
                            {
                                contains = true;
                                listIndex = plInventoryScript.items_here.IndexOf(item);
                            }
                        }
                    }
                    if (contains)
                    {
                        plInventoryScript.items_here[listIndex].GetComponent<Unusable>().quantity += facticialInventory[i].GetComponent<Unusable>().quantity;
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        facticialInventory[i].transform.parent = plInventoryScript.gameObject.transform;
                        plInventoryScript.items_here.Add(facticialInventory[i]);
                    }
                    facticialInventory.Remove(facticialInventory[i]);
                    containment.Remove(containment[i]);
                }
            }

            // что есть у игрока
            for (int i = 0; i < plInventoryScript.items_here.Count; i++)
            {
                var item = plInventoryScript.items_here[i];
                var itemScript = plInventoryScript.items_here[i].GetComponent<Grabbable>();
                if (!itemScript.tradeable)
                {
                    GUI.Label(new Rect(Screen.width * 0.26f, Screen.height * 0.1f + i * 20f, 200f, 20f), item.GetComponent<Unusable>().quantity.ToString());
                    GUI.Button(new Rect(Screen.width * 0.1f, Screen.height * 0.1f + i * 20f, Screen.width * 0.16f, 20f), itemScript.item_name + "(" + item.GetComponent<Unusable>().quantity + ")");
                }
                else
                {
                    GUI.Label(new Rect(Screen.width * 0.26f, Screen.height * 0.1f + i * 20f, 200f, 20f), itemScript.cost.ToString());
                    if (GUI.Button(new Rect(Screen.width * 0.1f, Screen.height * 0.1f + i * 20f, Screen.width * 0.16f, 20f), itemScript.item_name))
                    {
                        facticialInventory.Add(item);
                        containment.Add(item);
                        item.transform.parent = this.transform;
                        plInventoryScript.items_here.Remove(item);
                    }
                }

            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            player = collision.gameObject;
            isOpened = true;
        }
    }
}
