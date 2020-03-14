using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject goldPrefab;
    public List<GameObject> armor, weapon, consume, other;
    public List<GameObject> items_here;
    private Lighting timeCounter;
    public bool inventory_is_opened = false;
    private GUIStyle inv_style;
    private GameObject styles;
    private GameObject personality,reaction_system, condition;
    private Reaction reactionScript;
    private Personality personalityScript;
    private GUIStyles stylesScript;
    private string origin = "Неизвестное происхождение";
    public float maxWeigth = 70.0f, currentWeigth = 0f;
    private enum Windows
    {
        armor,weapon,consume,other
    }
    [SerializeField]Windows current_window;
    public bool draw = false;
    // Start is called before the first frame update
    void Start()
    {
        // добавление предмета золото в инвентарь
        var contains = false;
        var listIndex = -1;
        foreach (var item in items_here)
        {
            var unusScript = item.GetComponent<Unusable>();
            if (unusScript != null)
            {
                if (unusScript.item_name == "Золото")
                {
                    contains = true;
                    listIndex = items_here.IndexOf(item);
                }
            }
        }
        GetComponent<Inventory>().inventory_is_opened = false;
        if (contains)
        {
            items_here[listIndex].GetComponent<Unusable>().quantity += 0;
        }
        else
        {
            var trueGold = Instantiate(goldPrefab, this.transform);
            items_here.Add(trueGold);
        }

        //инициализация
        condition = GameObject.Find("Condition");
        timeCounter = GameObject.Find("TimeCounter").GetComponent<Lighting>();
        styles = GameObject.Find("GUIStyles");
        stylesScript = styles.GetComponent<GUIStyles>();
        inv_style = GameObject.Find("Main Camera").GetComponent<MainCamera>().gUISTYLE;
        personality = GameObject.Find("Personality");
        personalityScript = personality.GetComponent<Personality>();
        reaction_system = GameObject.Find("ReactionSystem");
        reactionScript = reaction_system.GetComponent<Reaction>();

        switch (personality.GetComponent<Personality>().myorigin)
        {
            case Personality.origin.unknown:
                origin = "Неизвестное происхождение";
                break;
            case Personality.origin.necromaster:
                origin = "Эльф-некромант";
                break;
            case Personality.origin.giant_moscuitte:
                origin = "Гигантский комар";
                break;
            case Personality.origin.deer:
                origin = "Стенографист - олень";
                break;
            case Personality.origin.bomj:
                origin = "Призрак";
                break;
            case Personality.origin.blind_dwarf:
                origin = "Слепой гном-кузнец";
                break;
            default:
                origin = "Плохой человек.";
                break;
            }
    }
    private void OnGUI()
    {
        GUI.skin = stylesScript.skins[0];
        if (inventory_is_opened)
        {
            GUI.Box(new Rect(0f, 0f, Screen.width * 0.3f, Screen.height), "");
            // имя, происхождение, время, броня, урон
            GUI.Label(new Rect(Screen.width * 0.005f, Screen.height * 0.03f, 150f, 150f), personality.GetComponent<Personality>().Character_name + ", " + personality.GetComponent<Personality>().Character_age); // имя
            GUI.Label(new Rect(Screen.width * 0.005f, Screen.height * 0.09f, 200f, 150f), origin);
            GUI.Label(new Rect(Screen.width * 0.18f, Screen.height * 0.03f, 150f, 20f),"Время:" + timeCounter.hours.ToString() + ":" + timeCounter.minutes.ToString());
            GUI.Box(new Rect(Screen.width * 0.18f, Screen.height * 0.03f + 20f, 20f, 20f),"",stylesScript.styles[10]); // иконка веса
            GUI.Label(new Rect(Screen.width * 0.18f + 20f, Screen.height * 0.03f + 20f, 150f, 20f), currentWeigth.ToString() + "/" + maxWeigth.ToString());
            var conditionScript = condition.GetComponent<Condition>();
            GUI.Label(new Rect(Screen.width * 0.18f, Screen.height * 0.03f + 40f, 150f, 20f), "Броня:" + conditionScript.armor.ToString());
            GUI.Label(new Rect(Screen.width * 0.18f, Screen.height * 0.03f + 60f, 150f, 20f), "Урон:" + conditionScript.damage);
            GUI.Label(new Rect(Screen.width * 0.18f, Screen.height * 0.03f + 80f, 150f, 20f), "Здоровье: " + conditionScript.hp);
            /* инвентарь
            1 - броня, 2 - оружие, 3 - еда и зелья, 4 - остальное
            */

            // Кнопки
            if (GUI.Button(new Rect(Screen.width * 0.005f, Screen.height * 0.14f, 20f, 20f), "", stylesScript.styles[0]))
            {
                current_window = Windows.armor;
            }
            if (GUI.Button(new Rect(Screen.width * 0.005f + 20f, Screen.height * 0.14f, 20f, 20f), "", stylesScript.styles[1]))
            {
                current_window = Windows.weapon;
            }
            if (GUI.Button(new Rect(Screen.width * 0.005f + 40f, Screen.height * 0.14f, 20f, 20f), "", stylesScript.styles[2]))
            {
                current_window = Windows.consume;
            }
            if (GUI.Button(new Rect(Screen.width * 0.005f + 60f, Screen.height * 0.14f, 20f, 20f), "", stylesScript.styles[3]))
            {
                current_window = Windows.other;
            }

            /*
            // Иконки эффективности и цены
            GUI.Box(new Rect(Screen.width * 0.005f + 120f + (Screen.width * 0.005f + 60f)/2, Screen.height * 0.14f, 20f, 20f), "", styles.GetComponent<GUIStyles>().styles[5]);
            GUI.Box(new Rect(Screen.width * 0.005f + 140f + (Screen.width * 0.005f + 60f)/2, Screen.height * 0.14f, 20f, 20f), "", styles.GetComponent<GUIStyles>().styles[6]); */

            //окно экипировки
            GUI.Box(new Rect(Screen.width * 0.25f, Screen.height * 0.03f, 40f, Screen.height*0.06f + 20f),"", stylesScript.styles[4]);

            if (!(personality.GetComponent<Personality>().armor_head == null))
                GUI.Label(new Rect(Screen.width * 0.25f, 0f * 0.14f + 60f, Screen.width * 0.048f, 40f), personalityScript.armor_head.GetComponent<Usable>().item_name);
            else
                GUI.Label(new Rect(Screen.width * 0.25f, 0f* 0.14f + 60f, Screen.width * 0.048f, 20f), "");

            if (!(personality.GetComponent<Personality>().armor_body == null))
                GUI.Label(new Rect(Screen.width * 0.25f, 0* 0.14f + 100f, Screen.width * 0.048f, 40f), personalityScript.armor_body.GetComponent<Usable>().item_name);
            else
                GUI.Label(new Rect(Screen.width * 0.25f, 0* 0.14f + 100f, Screen.width * 0.048f, 20f), "");

            if (!(personality.GetComponent<Personality>().armor_arms == null))
                GUI.Label(new Rect(Screen.width * 0.25f, 0* 0.14f + 140f, Screen.width * 0.048f, 40f), personalityScript.armor_arms.GetComponent<Usable>().item_name);
            else
                GUI.Label(new Rect(Screen.width * 0.25f, 0* 0.14f + 140f, Screen.width * 0.048f, 20f), "");

            if (!(personality.GetComponent<Personality>().armor_legs == null))
                GUI.Label(new Rect(Screen.width * 0.25f, 0* 0.14f + 180f, Screen.width * 0.048f, 40f), personalityScript.armor_legs.GetComponent<Usable>().item_name);
            else
                GUI.Label(new Rect(Screen.width * 0.25f, 0 * 0.14f + 180f, Screen.width * 0.048f, 20f), "");

            if (!(personality.GetComponent<Personality>().weapon == null))
                GUI.Label(new Rect(Screen.width * 0.25f, Screen.height * 0.14f + 220f, Screen.width * 0.048f, 40f), personalityScript.weapon.GetComponent<Usable>().item_name);
            else
                GUI.Label(new Rect(Screen.width * 0.25f, Screen.height * 0.14f + 220f, Screen.width * 0.048f, 20f), "");


            // списки предметов по кнопкам
            if (current_window.Equals(Windows.armor) )
                {
                if (armor.Count == 0)
                    GUI.Label(new Rect(Screen.width * 0.005f, Screen.height * 0.2f, Screen.width * 0.3f, 20f), "У вас нет брони.");
                else
                {
                    GUI.Box(new Rect(Screen.width * 0.005f + 120f + (Screen.width * 0.005f + 60f) / 2, Screen.height * 0.14f, 20f, 20f), "", stylesScript.styles[5]);
                    GUI.Box(new Rect(Screen.width * 0.005f + 160f + (Screen.width * 0.005f + 60f) / 2, Screen.height * 0.14f, 20f, 20f), "", stylesScript.styles[6]);
                    for (int i = 0; i < armor.Count; i++)
                    {
                        var armorScript = armor[i].GetComponent<Armor>();
                        GUI.Label(new Rect(Screen.width * 0.005f + 120f + (Screen.width * 0.005f + 60f) / 2, Screen.height * 0.2f + i * 20f, Screen.width * 0.1f, 20f), armorScript.resist.ToString());
                        GUI.Label(new Rect(Screen.width * 0.005f + 160f + (Screen.width * 0.005f + 60f) / 2, Screen.height * 0.2f + i * 20f, Screen.width * 0.1f, 20f), armorScript.cost.ToString());
                        if (GUI.Button(new Rect(Screen.width * 0.005f, Screen.height * 0.2f + i * 20f, Screen.width * 0.1f, 20f), armorScript.item_name))
                        {
                            if (armorScript.armor_slot == Armor.armor_equip_slot.head)
                            {
                                personalityScript.SetHead(armor[i]);
                            }
                            else if(armorScript.armor_slot == Armor.armor_equip_slot.body)
                            {
                                personalityScript.SetBody(armor[i]);
                            }
                            else if (armorScript.armor_slot == Armor.armor_equip_slot.arms)
                            {
                                personalityScript.SetArms(armor[i]);
                            }
                            else
                            {
                                personalityScript.SetLegs(armor[i]);
                            }
                            if (!reactionScript.contained)
                            {
                                reactionScript.message = armorScript.commentary;
                                reactionScript.contained = true;
                            }
                            armor.Remove(armor[i]);
                            inventory_is_opened = false;
                        }
                        // кнопка удаления
                        if(GUI.Button(new Rect(Screen.width * 0.005f + 160f + (Screen.width * 0.005f + 60f) / 2 + Screen.width*0.1f, Screen.height*0.2f+i*20f,20f,20f),"", stylesScript.styles[9]))
                        {
                            RemoveFromInventory(armor[i], armor, i);
                        }
                    }
                }
                }
                if (current_window.Equals(Windows.weapon))
                {
                if (weapon.Count == 0)
                    GUI.Label(new Rect(Screen.width * 0.005f, Screen.height * 0.2f, Screen.width * 0.3f, 20f), "У вас нет оружия.");
                else
                {
                    GUI.Box(new Rect(Screen.width * 0.005f + 120f + (Screen.width * 0.005f + 60f) / 2, Screen.height * 0.14f, 20f, 20f), "", stylesScript.styles[5]);
                    GUI.Box(new Rect(Screen.width * 0.005f + 160f + (Screen.width * 0.005f + 60f) / 2, Screen.height * 0.14f, 20f, 20f), "", stylesScript.styles[6]);
                    for (int i = 0; i < weapon.Count; i++)
                    {
                        var weapScript = weapon[i].GetComponent<Weapon>();
                        GUI.Label(new Rect(Screen.width * 0.005f + 120f + (Screen.width * 0.005f + 60f) / 2, Screen.height * 0.2f + i * 20f, Screen.width * 0.1f, 20f), weapScript.damage.ToString());
                        GUI.Label(new Rect(Screen.width * 0.005f + 160f + (Screen.width * 0.005f + 60f) / 2, Screen.height * 0.2f + i * 20f, Screen.width * 0.1f, 20f), weapScript.cost.ToString());
                        if (GUI.Button(new Rect(Screen.width * 0.005f, Screen.height * 0.2f + i * 20f, Screen.width * 0.1f, 20f), weapScript.item_name))
                        {
                            personalityScript.SetWeapon(weapon[i]);
                            if (!reactionScript.contained)
                            {
                                reactionScript.message = weapScript.commentary;
                                reactionScript.contained = true;
                            }
                            weapon.Remove(weapon[i]);
                            inventory_is_opened = false;
                        }
                        if (GUI.Button(new Rect(Screen.width * 0.005f + 160f + (Screen.width * 0.005f + 60f) / 2 + Screen.width * 0.1f, Screen.height * 0.2f + i * 20f, 20f, 20f), "", stylesScript.styles[9]))
                        {
                            RemoveFromInventory(weapon[i], weapon, i);
                        }
                    }
                }
                }
                if (current_window.Equals(Windows.consume))
                {
                if (consume.Count == 0)
                    GUI.Label(new Rect(Screen.width * 0.005f, Screen.height * 0.2f, Screen.width * 0.3f, 20f), "У вас нет расходуемых предметов.");
                else
                {
                    GUI.Box(new Rect(Screen.width * 0.005f + 160f + (Screen.width * 0.005f + 60f) / 2, Screen.height * 0.14f, 20f, 20f), "", stylesScript.styles[6]);
                    for (int i = 0; i < consume.Count; i++)
                    {
                        var consScript = consume[i].GetComponent<Consumable>();
                        if(GUI.Button(new Rect(Screen.width * 0.005f, Screen.height * 0.2f + i * 0.3f, Screen.width * 0.1f, 20f), consScript.item_name))
                        {
                            personalityScript.EatConsume(consume[i]);
                            if (!reactionScript.contained)
                            {
                                reactionScript.message = consScript.commentary;
                                reactionScript.contained = true;
                            }
                            consume.Remove(consume[i]);
                            inventory_is_opened = false;
                        }
                        if (GUI.Button(new Rect(Screen.width * 0.005f + 160f + (Screen.width * 0.005f + 60f) / 2 + Screen.width * 0.1f, Screen.height * 0.2f + i * 20f, 20f, 20f), "", stylesScript.styles[9]))
                        {
                            RemoveFromInventory(consume[i], consume, i);
                        }
                    }
                }
                }

                if (current_window.Equals(Windows.other))
                {
                if (other.Count == 0)
                    GUI.Label(new Rect(Screen.width * 0.005f, Screen.height * 0.2f, Screen.width * 0.3f, 20f), "У вас нет ничего такого разного, что можно было бы продать.");
                else
                {
                    GUI.Box(new Rect(Screen.width * 0.005f + 160f + (Screen.width * 0.005f + 60f) / 2, Screen.height * 0.14f * 0.3f, 20f, 20f), "", stylesScript.styles[6]);
                    for (int i = 0; i < other.Count; i++)
                    {
                        var othScript = other[i].GetComponent<Unusable>();
                        if (GUI.Button(new Rect(Screen.width * 0.005f, Screen.height * 0.2f + i * 20f, Screen.width * 0.1f, 20f), othScript.item_name + "(" + othScript.quantity + ")"))
                        {
                            if (!reactionScript.contained)
                            {
                                reactionScript.message = othScript.commentary;
                                reactionScript.contained = true;
                            }
                        }
                        if (other[i].GetComponent<Grabbable>().tradeable && GUI.Button(new Rect(Screen.width * 0.005f + 160f + (Screen.width * 0.005f + 60f) / 2 + Screen.width * 0.1f, Screen.height * 0.2f + i * 20f, 20f, 20f), "", stylesScript.styles[9]))
                        {
                            RemoveFromInventory(other[i],other,i);
                        }
                        GUI.Label(new Rect(Screen.width * 0.005f + 160f + (Screen.width * 0.005f + 60f) / 2, Screen.height * 0.2f + i * 20f, Screen.width * 0.1f, 20f), othScript.cost.ToString() + " * " + othScript.quantity.ToString());
                    }
                }
            }
        }
    }
    
    private void Update()
    {
        currentWeigth = (float)System.Math.Round(currentWeigth, 3);
        if (Input.GetKey(KeyCode.I))
        {
            if (!(GameObject.Find("dialogue_Window")!=null))
            {
                inventory_is_opened = !inventory_is_opened;
                System.Threading.Thread.Sleep(250);
            }
            else
            {
                inventory_is_opened = false;
            }
        }
        if (inventory_is_opened)
        {
            foreach(var item in items_here)
            {
                var grabScript = item.GetComponent<Grabbable>();
                if (grabScript.type == Grabbable.Type.armor && !draw)
                {
                    armor.Add(item);
                    
                }
                else if (grabScript.type == Grabbable.Type.weapon && !draw)
                {
                    weapon.Add(item);
                    
                }
                else if (grabScript.type == Grabbable.Type.consume && !draw)
                {
                    consume.Add(item);
                    
                }
                else if (grabScript.type == Grabbable.Type.other && !draw)
                {
                    other.Add(item);
                    
                }
                
            }
            draw = true;
        }
        else
        {
            armor.Clear();
            weapon.Clear();
            consume.Clear();
            other.Clear();
            draw = false;
        }
    }

    void RemoveFromInventory(GameObject remItem, List<GameObject> list, int listIndex)
    {
        var item = list[listIndex];
        currentWeigth -= item.GetComponent<Grabbable>().weigth;
        items_here.Remove(item);
        list.Remove(item);
        Destroy(item.gameObject);
        inventory_is_opened = false;
    }
}