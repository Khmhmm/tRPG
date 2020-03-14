using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MiscObjects
{
    protected GameObject inventory;
    protected Inventory invScript;
    public string item_name = "Default item";
    public float weigth=1f;
    public int cost = 0;
    public bool tradeable = true;
    public int id;
    public enum Type
    {
        armor,weapon,consume,other
    }
    public Type type;
    protected bool collected = false;
    // Start is called before the first frame update
    protected void Start()
    {
        inventory = GameObject.Find("Inventory");
    }


    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (playerIsHere && Input.GetKey(KeyCode.E))
        {
            if(inventory.GetComponent<Inventory>().currentWeigth + weigth < inventory.GetComponent<Inventory>().maxWeigth)
            {
                inventory.GetComponent<Inventory>().inventory_is_opened = false;
                inventory.GetComponent<Inventory>().items_here.Add(this.gameObject);
                inventory.GetComponent<Inventory>().currentWeigth += this.weigth;
                transform.parent = inventory.transform;
                transform.localPosition = new Vector3(0f, 0f, 15f);
                transform.localScale = new Vector3(0.3f, 0.3f, 1f);
                this.gameObject.SetActive(false);
                try
                {
                    foreach (var item in inventory.GetComponent<Inventory>().items_here)
                    {
                        if (item.GetComponent<Usable>().equiped && item.GetComponent<Usable>() != null)
                        {
                            inventory.GetComponent<Inventory>().items_here.Remove(item);
                        }
                    }
                }
                catch (System.NullReferenceException e)
                {
                    //так и не разобрался, как выключить эксцепшн. удачи, ревьюер)
                    Debug.Log("List was modified" + e);
                }
                finally
                {

                }
            }
            else
            {

            }
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerIsHere = true;
            if (Input.GetKey(KeyCode.E))
            {
                var reactionSystem = collision.transform.GetChild(1);
                if (inventory.GetComponent<Inventory>().currentWeigth + weigth < inventory.GetComponent<Inventory>().maxWeigth)
                {
                    if (!reactionSystem.GetComponent<Reaction>().contained)
                    {
                        reactionSystem.GetComponent<Reaction>().message = commentary;
                        reactionSystem.GetComponent<Reaction>().contained = true;
                    }
                    var personality = collision.transform.GetChild(3);
                    personality.GetComponent<Personality>().weapon_in_arms = false;
                }
                else
                {
                    if (!reactionSystem.GetComponent<Reaction>().contained)
                    {
                        reactionSystem.GetComponent<Reaction>().message = "Слишком тяжело.";
                        reactionSystem.GetComponent<Reaction>().contained = true;
                    }
                }
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerIsHere = false;
            if (Input.GetKey(KeyCode.E))
            {
                var reactionSystem = other.transform.Find("ReactionSystem");
                if (inventory.GetComponent<Inventory>().currentWeigth + weigth < inventory.GetComponent<Inventory>().maxWeigth)
                {
                    if (!reactionSystem.GetComponent<Reaction>().contained)
                    {
                        reactionSystem.GetComponent<Reaction>().message = commentary;
                        reactionSystem.GetComponent<Reaction>().contained = true;
                    }
                    collected = true;
                }

                else
                {
                    if (!reactionSystem.GetComponent<Reaction>().contained)
                    {
                        reactionSystem.GetComponent<Reaction>().message = "Слишком тяжело.";
                        reactionSystem.GetComponent<Reaction>().contained = true;
                    }
                }
            }
        }

    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.E))
        {
            var reactionSystem = collision.transform.Find("ReactionSystem");
            if (inventory.GetComponent<Inventory>().currentWeigth + weigth < inventory.GetComponent<Inventory>().maxWeigth)
            {
                if (!reactionSystem.GetComponent<Reaction>().contained)
                {
                    reactionSystem.GetComponent<Reaction>().message = commentary;
                    reactionSystem.GetComponent<Reaction>().contained = true;
                }
            }
            else
            {
                if (!reactionSystem.GetComponent<Reaction>().contained)
                {
                    reactionSystem.GetComponent<Reaction>().message = "Слишком тяжело.";
                    reactionSystem.GetComponent<Reaction>().contained = true;
                }
            }
        }
    }
}

