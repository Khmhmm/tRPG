using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacing : MonoBehaviour
{
    public Collider2D coc;
    private Animator anim;
    private GameObject condition;
    private Condition condScript;
    private float startSpeed;
    public bool delay = false, damaged = false, notAFigth = false, notAFigthDelay = false;
    void Start()
    {
        anim = transform.parent.GetComponent<movement>().anim;
        condition = transform.parent.Find("Condition").gameObject;
        condScript = condition.GetComponent<Condition>();
        coc = this.GetComponent<CapsuleCollider2D>();
        coc.enabled = false;
    }

    private void LateUpdate()
    {   
        if (notAFigth && condScript.current_status == Condition.Status.fighting && !notAFigthDelay)
        {
            StartCoroutine("ChangeStatus");
            notAFigthDelay = true;
        }
        else if (!notAFigth)
        {
            StopCoroutine("ChangeStatus");
            notAFigthDelay = false;
        }
    }

    void FixedUpdate()
    {
        if (condScript.current_status != Condition.Status.dead && condScript.current_status != Condition.Status.dialogue)
        {
            if (Input.GetKey(KeyCode.Mouse0) && !delay && condScript.current_status != Condition.Status.dead)
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Attack", true);
                coc.enabled = true;
                delay = true;
                StartCoroutine("Delay");
            }
            else if (delay)
            {
                coc.enabled = true;
            }
            else
            {
                coc.enabled = false;
                damaged = false;
                anim.SetBool("Attack", false);
            }
        }
    }
    void OnTriggerStay2D(Collider2D enemy)
    {
        if (enemy.tag == "Enemy" && !damaged)
        {
           damaged = true;
           enemy.GetComponent<Enemy>().hp -= condition.GetComponent<Condition>().damage;
           //дальность отбрасывания прибавляется, помноженная на 5, к базовой силе в 50 Ньютон. Вектор направлен от игрока к врагу.
           enemy.GetComponent<Rigidbody2D>().AddForce(-(150f + transform.parent.transform.GetComponentInChildren<Condition>().damage * 5) * new Vector2((transform.position.x - enemy.transform.position.x),(transform.position.y - enemy.transform.position.y)).normalized);
        }
        else if(enemy.tag== "CanBecomeAnEnemy")
        {
            enemy.tag = "Enemy";
        }
        else
        {
            notAFigth = true;
        }

    }
    
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.52f);
        delay = false;
        /* не уверен, куда именно нужно засунуть damaged. Нужна для того, чтобы просчитывался лишь один удар во время анимации удара
        *damaged = false;
        */
    }

    IEnumerator ChangeStatus()
    {
        yield return new WaitForSeconds(9f);
        if (notAFigth)
        {
            condScript.current_status = Condition.Status.freewalking;
            notAFigthDelay = false;
        }
    } 
}
