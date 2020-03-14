using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscObjects : MonoBehaviour
{
    public string commentary = "Хлам";
    public bool playerIsHere = false;
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            playerIsHere = true;
            if (Input.GetKey(KeyCode.E))
            {
                var reactionSystem = collision.transform.GetChild(1);
                if (!reactionSystem.GetComponent<Reaction>().contained)
                {
                    reactionSystem.GetComponent<Reaction>().message = commentary;
                    reactionSystem.GetComponent<Reaction>().contained = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerIsHere = false;
        }
    }
}
