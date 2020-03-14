using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool delay = false;
    public bool entered = false;
    public GameObject father;

    private void OnEnable()
    {
        delay = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag=="Player" && Input.GetKey(KeyCode.E)&& !delay)
        {
            collision.transform.position += new Vector3(0f, 0f, -38f);
            entered = true;
            father.GetComponent<House>().interrior.GetComponent<Interrior>().inside = true;
            delay = true;
            gameObject.SetActive(false);
            System.Threading.Thread.Sleep(150);
        }
    }
}
