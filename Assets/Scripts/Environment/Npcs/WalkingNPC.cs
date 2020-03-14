using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingNPC : MonoBehaviour
{
    public float moveSpeed = 1f;
    private float currentMoveSpeed = 0f;
    private bool lookRight = true, delay=false;
    public Animator animator;
    private Vector3 Startposition, GoTo;
    // Start is called before the first frame update
    void Start()
    {
        Startposition = transform.position;
        GoTo = new Vector3(Startposition.x + 2f, Startposition.y, Startposition.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GoThere();
        if (lookRight)
        {
            this.transform.localScale = new Vector3(3f, 3f);
        }
        else
        {
            this.transform.localScale = new Vector3(-3f, 3f);
        }

        if (currentMoveSpeed != 0)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        delay = false;
        lookRight = !lookRight;
    }

    void GoThere()
    {
        if(transform.position.x<GoTo.x && lookRight)
        {
            currentMoveSpeed += (currentMoveSpeed < moveSpeed) ? 0.025f : 0f;
            transform.Translate(new Vector3(currentMoveSpeed/100f, 0f));
            if(GoTo.x - transform.position.x < 0.15f && !delay)
            {
                StartCoroutine("Delay");
                delay = true;
                currentMoveSpeed = 0;
            }
        }
        else if(!lookRight && transform.position.x > Startposition.x)
        {
            currentMoveSpeed += (currentMoveSpeed < moveSpeed) ? 0.25f : 0f;
            transform.Translate(new Vector3(-currentMoveSpeed/100f, 0f));
            if (transform.position.x - Startposition.x < 0.15f && !delay)
            {
                StartCoroutine("Delay");
                delay = true;
                currentMoveSpeed = 0;
            }
        }
        else
        {
            currentMoveSpeed = 0f;
        }
    }
}
