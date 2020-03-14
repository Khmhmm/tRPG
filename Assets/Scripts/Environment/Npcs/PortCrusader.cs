using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortCrusader : MonoBehaviour
{
    public bool changed = false;
    GameObject[] Friends;
    // Start is called before the first frame update
    void Start()
    {
        Friends = GameObject.FindGameObjectsWithTag("CanBecomeAnEnemy");
        InvokeRepeating("CheckPlayerCondition", 10f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        //Тег меняется на Enemy:
        // Включить компонент Enemy, отключить DefaultNPC и WalkingNPC
        // Условия: Игрок ударил стражника, либо вышел за пределы порта без регистрации
        if (tag == "Enemy" && !changed)
        {
            foreach(var friend in Friends)
            {
                friend.tag = "Enemy";
            }
            GetComponent<Enemy>().enabled = true;
            GetComponent<DefaultNPC>().enabled = false;
            GetComponent<WalkingNPC>().enabled = false;
            changed = true;
        }
    }

    void CheckPlayerCondition()
    {
        if(transform.parent.transform.parent.GetComponent<MainObject>().player.GetComponentInChildren<Condition>().hp <= 0f)
        {
            tag = "CanBecomeAnEnemy";
            GetComponent<Enemy>().enabled = false;
            GetComponent<DefaultNPC>().enabled = true;
            GetComponent<WalkingNPC>().enabled = true;
        }
    }
}
