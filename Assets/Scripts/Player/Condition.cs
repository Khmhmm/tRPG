using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour
{
    private GUIStyles guiStyles;
    public float maxhp=100f,hp=100f, armor,damage=0f, weapdamage=0f, handdamage = 1f;
    public int level=1, exp=1;
    //скалирование уровня по формуле: lvl = lvl*90 + lvl^lvl - (lvl-1)^lvl
    //урон расчитывается так: урон от оружия (берется из экипированного в Personality объекта) + урон с кулака, который скалируется с уровнем.

    // public int strength = 1, agillity = 1, intelligence = 1, stamina = 1;
    public enum Status
    {
        freewalking,fighting,dialogue, dead
    }
    public Status current_status = Status.freewalking;
    // Start is called before the first frame update
    void Start()
    {
        guiStyles = GameObject.Find("GUIStyles").GetComponent<GUIStyles>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (exp >= level * 90 + (int)(Mathf.Pow(level, level) - Mathf.Pow(level - 1, level)))
        {
            exp -= level * 90 + (int)(Mathf.Pow(level, level) - Mathf.Pow(level - 1, level));
            ++level;
            maxhp += level * 100;
            handdamage = Mathf.Pow(2, level);
        }
    }

    private void LateUpdate()
    {
        hp = (float)System.Math.Round(hp, 3);
        if (hp <= 0)
        {
            //                                            |
            //SceneManager.LoadScene(deathscreen) вместо \|/
            //                                            V
            current_status = Status.dead;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && current_status != Status.dialogue)
        {
            current_status = Status.fighting;
        } 
        else
        {
            transform.parent.gameObject.GetComponentInChildren<Attacing>().notAFigth = true;
        }
    }

    private void OnGUI()
    {
        if (current_status == Status.fighting)
        {
            var onScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
            GUI.color = Color.red;
            GUI.Box(new Rect(Screen.width, Screen.height - 30f, -100f, 30f), "");
            int percent = (int)(100 * hp / maxhp);
            GUI.Label(new Rect(Screen.width, Screen.height - 30f, -percent, 30f), "", guiStyles.styles[11]);
        }
    }
}
