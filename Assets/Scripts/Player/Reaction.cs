using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction : MonoBehaviour
{
    private GameObject player;
    public bool contained = false;
    private bool delay = false;
    public string message;
    private GUIStyles stylesScript;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject;
        stylesScript = GameObject.FindGameObjectWithTag("MainCamera").transform.Find("GUIStyles").GetComponent<GUIStyles>();
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
        GUI.skin = stylesScript.skins[2];
        var onScreenPoint = Camera.main.WorldToScreenPoint(player.transform.position);
        GUI.Label(new Rect(onScreenPoint.x * 1.012f,Screen.height*0.95f - onScreenPoint.y + 0.01f,250f,40f),message);
    }

    public void SetReplics(string message)
    {
        if (!contained)
        {
            this.message = message;
            contained = true;
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
        message = "";
        delay = false;
        contained = false;
    }
}
