using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultNPC : MonoBehaviour
{
    private GameObject player;
    public bool contained = false;
    private bool delay = false;
    public string message,Replics1,Replics2;
    private bool first_replics = true;
    public bool player_isHere = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (contained && !delay)
        {
            StartCoroutine("Delay");
            delay = true;
        }
    }

    private void OnGUI()
    {
        var onScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        GUI.Label(new Rect(onScreenPoint.x * 1.009f, Screen.height * 0.95f - onScreenPoint.y, 250f, 20f), message);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(4f);
        message = "";
        delay = false;
        contained = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Player" && !contained)
        {
            contained = true;
            message = Replics();
            player_isHere = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player_isHere = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player_isHere = false;
        }
    }

    private string Replics()
    {
        first_replics = !first_replics;
        return (first_replics) ? Replics1 : Replics2;
    }
}
