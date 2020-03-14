using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    bool delay = false;
    private GameObject thisHouseDoor;
    public GameObject father;
    private void Start()
    {
        father = transform.parent.GetComponent<Interrior>().father;
        thisHouseDoor = father.GetComponent<House>().door;
    }
    private void OnEnable()
    {
        delay = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E) && !delay)
        {
            collision.transform.position += new Vector3(0f, 0f, 38f);
            transform.parent.GetComponent<Interrior>().inside = false;
            thisHouseDoor.GetComponent<Door>().entered = false;
            delay = true;
            gameObject.SetActive(false);
            System.Threading.Thread.Sleep(250);
        }
    }
}
