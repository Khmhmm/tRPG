using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personality : MonoBehaviour
{
    public string Character_name, Character_age, Character_level, Character_exp;
    public GameObject armor_head, armor_body, armor_arms, armor_legs, weapon, inventory;
    public List<GameObject> effects;
    private GameObject condition;
    private Animator anim;
    public bool weapon_in_arms = false;
    bool weaponDelay = false;
    public enum origin
    {
       unknown, necromaster, bomj, deer, blind_dwarf, giant_moscuitte
    }
    public origin myorigin;

    public void Start()
    {
        anim = transform.parent.GetComponent<movement>().anim;
        inventory = GameObject.Find("Inventory");
        condition = GameObject.Find("Condition");
    }

    IEnumerator WeaponDelay()
    {
        yield return new WaitForSeconds(0.2f);
        weaponDelay = false;
    }
    
    public void FixedUpdate()
    {
        if(weapon!=null)
            weapon.transform.localPosition = new Vector3(0f, 0f, 15f);
        var cond = transform.parent.GetComponentInChildren<Condition>();
        // визуализация оружия. при weapon_in_arms = true - оружие в руках
        if (Input.GetKey(KeyCode.R) && weapon!=null && !weaponDelay)
        {
            weaponDelay = true;
            StartCoroutine("WeaponDelay");
            weapon_in_arms = !weapon_in_arms;
        }
        else if (weapon_in_arms)
        {
            cond.damage = cond.handdamage + cond.weapdamage;
        }
        else
        {
            cond.damage = cond.handdamage;
        }
    }

    private void LateUpdate()
    {
        if (weapon_in_arms)
        {
            anim.SetBool("WithWeapon", true);
        }
        else
        {
            anim.SetBool("WithWeapon", false);
        }
          
        foreach(var ef in effects)
        {
            var effectScript = ef.GetComponent<Effects>();
            if (effectScript != null)
            {
                if (!effectScript.delay)
                {
                    Debug.Log("Effect was started");
                    effectScript.StartCoroutine("Effecting");
                    effectScript.delay = true;
                }
            }
        }
    }

    public void SetOrigin(origin _origin)
    {
        myorigin = _origin;
    }

    public void SetName(string _name)
    {
        Character_name = _name;
    }

    public void SetHead(GameObject _head_armor)
    {
        if (armor_head != null)
        {
            inventory.GetComponent<Inventory>().items_here.Add(armor_head);
            inventory.GetComponent<Inventory>().currentWeigth += armor_head.GetComponent<Grabbable>().weigth;
            armor_head.GetComponent<Usable>().equiped = false;
            transform.parent.transform.Find("Condition").gameObject.GetComponent<Condition>().armor -= armor_head.GetComponent<Armor>().resist;
        }
        armor_head = _head_armor;
        transform.parent.transform.Find("Condition").gameObject.GetComponent<Condition>().armor += armor_head.GetComponent<Armor>().resist;
        inventory.GetComponent<Inventory>().currentWeigth -= armor_head.GetComponent<Grabbable>().weigth;
        inventory.GetComponent<Inventory>().items_here.Remove(_head_armor);
        armor_head.GetComponent<Usable>().equiped = true;
    }

    public void SetBody(GameObject _body_armor)
    {
        if (armor_body != null)
        {
            inventory.GetComponent<Inventory>().items_here.Add(armor_body);
            armor_body.GetComponent<Usable>().equiped = false;
            inventory.GetComponent<Inventory>().currentWeigth += armor_body.GetComponent<Grabbable>().weigth;
            transform.parent.transform.Find("Condition").gameObject.GetComponent<Condition>().armor -= armor_body.GetComponent<Armor>().resist;
        }
        armor_body = _body_armor;
        transform.parent.transform.Find("Condition").gameObject.GetComponent<Condition>().armor += armor_body.GetComponent<Armor>().resist;
        inventory.GetComponent<Inventory>().currentWeigth -= armor_body.GetComponent<Grabbable>().weigth;
        inventory.GetComponent<Inventory>().items_here.Remove(_body_armor);
        armor_body.GetComponent<Usable>().equiped = true;
    }

    public void SetArms(GameObject _arms_armor)
    {
        if (armor_arms != null)
        {
            inventory.GetComponent<Inventory>().items_here.Add(armor_arms);
            inventory.GetComponent<Inventory>().currentWeigth += armor_arms.GetComponent<Grabbable>().weigth;
            armor_arms.GetComponent<Usable>().equiped = false;
            transform.parent.transform.Find("Condition").gameObject.GetComponent<Condition>().armor -= armor_arms.GetComponent<Armor>().resist;
        }
        armor_arms = _arms_armor;
        inventory.GetComponent<Inventory>().currentWeigth -= armor_body.GetComponent<Grabbable>().weigth;
        transform.parent.transform.Find("Condition").gameObject.GetComponent<Condition>().armor += armor_arms.GetComponent<Armor>().resist;
        inventory.GetComponent<Inventory>().items_here.Remove(_arms_armor);
        armor_arms.GetComponent<Usable>().equiped = true;
    }

    public void SetLegs(GameObject _legs_armor)
    {
        if (armor_legs != null)
        {
            inventory.GetComponent<Inventory>().items_here.Add(armor_legs);
            armor_legs.GetComponent<Usable>().equiped = false;
            transform.parent.transform.Find("Condition").gameObject.GetComponent<Condition>().armor += armor_legs.GetComponent<Armor>().resist;
        }
        armor_legs = _legs_armor;
        inventory.GetComponent<Inventory>().currentWeigth -= armor_legs.GetComponent<Grabbable>().weigth;
        transform.parent.transform.Find("Condition").gameObject.GetComponent<Condition>().armor += armor_legs.GetComponent<Armor>().resist;
        inventory.GetComponent<Inventory>().items_here.Remove(_legs_armor);
        armor_legs.GetComponent<Usable>().equiped = true;
    }

    public void SetWeapon(GameObject _weapon)
    {
        if (weapon != null)
        {
            transform.parent.transform.Find("Condition").gameObject.GetComponent<Condition>().weapdamage -= weapon.GetComponent<Weapon>().damage;
            inventory.GetComponent<Inventory>().items_here.Add(weapon);
            inventory.GetComponent<Inventory>().currentWeigth += weapon.GetComponent<Grabbable>().weigth;
            weapon.GetComponent<Usable>().equiped = false;
            weapon.gameObject.SetActive(false);
        }
        weapon = _weapon;
        inventory.GetComponent<Inventory>().currentWeigth -= weapon.GetComponent<Grabbable>().weigth;
        transform.parent.transform.Find("Condition").gameObject.GetComponent<Condition>().weapdamage += weapon.GetComponent<Weapon>().damage;
        inventory.GetComponent<Inventory>().items_here.Remove(_weapon);
        weapon.GetComponent<Usable>().equiped = true;
    }

    public void EatConsume(GameObject _consume)
    {
        try
        {
            var effect = _consume.GetComponent<Consumable>().effect;
            if (effect == null)
                throw new System.Exception("Unable to get effect");
            else
            {
                effects.Add(Instantiate(effect,this.transform));
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }
}
