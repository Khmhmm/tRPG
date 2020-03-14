using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    private GameObject player, condition;
    private Condition condScript;
    public Animator anim;
    private float speed;
    public float curentspeed;
    public bool right = true, isMoving = false;
    public int rot;
    void Start()
    {
        player = this.gameObject;
        rot = 1;
        curentspeed = 0.5f;
        speed = 3f;
        condition = transform.Find("Condition").gameObject;
        condScript = condition.GetComponent<Condition>();
    }

    private void Update()
    {
        if(condScript.current_status == Condition.Status.fighting)
        {
            speed = 1.78f + condScript.level/10f;
        }
        else
        {
            speed = 3f;
        }
        if (right)
        {
            transform.localScale = new Vector3(3f, 3f);
        }
        else
        {
            transform.localScale = new Vector3(-3f, 3f);
        }
    }

    void FixedUpdate()
    {
        if (transform.GetChild(4).gameObject.GetComponent<Condition>().current_status == Condition.Status.dialogue)
        {
            anim.SetBool("Walk", false);
        }

        if (isMoving)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        if (condition.GetComponent<Condition>().current_status != Condition.Status.dead)
        {
            if (condition.GetComponent<Condition>().current_status == Condition.Status.dialogue)
            {
                isMoving = false;
                anim.SetBool("Walk", false);
                anim.SetBool("Attack", false);
                return;
            }
            else if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, curentspeed, 0) * Time.deltaTime);
                rot = 1;
                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(curentspeed, 0, 0) * Time.deltaTime);
                rot = 2;
                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, -curentspeed, 0) * Time.deltaTime);
                rot = 3;
                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(-curentspeed, 0, 0) * Time.deltaTime);
                rot = 4;
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
            
            if (curentspeed < speed && isMoving)
            {
                curentspeed += 0.25f;
            }
            else if (curentspeed >= speed)
            {
                curentspeed = speed;
            }
            else if (!isMoving)
            {
                curentspeed = 0.9f;
            }

            if (rot == 2)
            {
                right = true;
            }
            else if (rot == 4)
            {
                right = false;
            }
        }
    }
}
