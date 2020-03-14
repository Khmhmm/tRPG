using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : DialogueableNPC
{
    public List<GameObject> traderInventory, facticialInventory;
    public GameObject playerInventory;
    public Inventory plInventoryScript;
    public string tradingReplics = "Поторгуем?";
    private bool tradingBox = false, inventorySpawned = false, draw = false;
    private GameObject styles;
    private GameObject personality, reaction_system, condition;
    private Reaction reactionScript;
    private Personality personalityScript;
    private GUIStyles stylesScript;
    public GameObject gold;
    public Unusable goldScript;
    private enum Windows
    {
        armor, weapon, consume, other
    }
    [SerializeField] Windows current_window;

    private void Start()
    {
        StartCoroutine("FindGold");
        condition = GameObject.Find("Condition");
        styles = GameObject.Find("GUIStyles");
        stylesScript = styles.GetComponent<GUIStyles>();
        personality = GameObject.Find("Personality");
        personalityScript = personality.GetComponent<Personality>();
        reaction_system = GameObject.Find("ReactionSystem");
        reactionScript = reaction_system.GetComponent<Reaction>();
        playerInventory = GameObject.Find("Inventory");
        plInventoryScript = playerInventory.GetComponent<Inventory>();
    }

    new void LateUpdate()
    {
        if (!inventorySpawned)
        {
            foreach(var item in traderInventory)
            {
                facticialInventory.Add(Instantiate(item, this.transform));
                item.SetActive(false);
            }
            inventorySpawned = true;
        }

        base.LateUpdate();
    }
    
        new void OnGUI()
        {
        GUI.skin = dialogueWindow.transform.parent.GetComponentInChildren<GUIStyles>().skins[1];
        if (dialogue)
        {
            if (tradingBox)
            {
                plInventoryScript.inventory_is_opened = true;
                plInventoryScript.inventory_is_opened = false;
                firstReplics = false;
                GUI.Box(new Rect(Screen.width * 0.1f, Screen.height * 0.001f, Screen.width * 0.8f, Screen.height * 0.8f), "");
                if (GUI.Button(new Rect(Screen.width * 0.9f - 20f, Screen.height * 0.001f, 20f, 20f), "",stylesScript.styles[8]))
                {
                    tradingBox = false;
                    firstReplics = true;
                    draw = false;
                }

                GUI.Box(new Rect(Screen.width * 0.26f, Screen.height * 0.001f, 20f, 20f), "",stylesScript.styles[6]);
                GUI.Box(new Rect(Screen.width * 0.86f, Screen.height * 0.001f,20f,20f),"",stylesScript.styles[6]);

                // что продает торговец
                for (int i=0;i<facticialInventory.Count;i++)
                {
                    var itemScript = facticialInventory[i].GetComponent<Grabbable>();
                    GUI.Label(new Rect(Screen.width*0.86f,Screen.height*0.1f+i*20f,200f,20f), itemScript.cost.ToString());
                    if(GUI.Button(new Rect(Screen.width * 0.7f, Screen.height * 0.1f + i * 20f, Screen.width * 0.15f, 20f),facticialInventory[i].GetComponent<Grabbable>().item_name))
                    {
                        if (itemScript.cost > goldScript.quantity)
                        {
                            //если предмет стоит больше, чем есть золота у игрока, то ничего не происходит
                            Debug.Log("You have not enough money");
                        }
                        else
                        {
                            facticialInventory[i].transform.parent = playerInventory.transform;
                            goldScript.quantity -= itemScript.cost;
                            plInventoryScript.items_here.Add(facticialInventory[i]);
                            facticialInventory.Remove(facticialInventory[i]);
                            traderInventory.Remove(traderInventory[i]);
                        }
                    }
                }

                // что есть у игрока
                for(int i = 0; i < plInventoryScript.items_here.Count; i++)
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
                                goldScript.quantity += itemScript.cost;
                                itemScript.cost += (int)(1.5f * itemScript.cost);
                                facticialInventory.Add(item);
                                traderInventory.Add(item);
                                item.transform.parent = this.transform;
                                plInventoryScript.items_here.Remove(item);
                        }
                    }

                }
            }
        

            else
            {
                if (firstReplics)
                {
                    if (GUI.Button(new Rect(Screen.width * 0.7f, Screen.height * 0.1f, Screen.width * 0.25f, 20f), tradingReplics))
                    {
                        //открывается окно торговли
                        tradingBox = true;
                    }
                }
                base.OnGUI();
            }

        }
    }
   

    IEnumerator FindGold()
    {
        yield return new WaitForSeconds(1f);
        gold = GameObject.Find("Coin Variant(Clone)");
        goldScript = gold.GetComponent<Unusable>();
    }
}
