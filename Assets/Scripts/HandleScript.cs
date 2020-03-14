using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleScript : MonoBehaviour
{
    public bool WorldMessage = false;
    public string speaker = "";
    public string message = "";
    public float time_interval = 0.1f;
    private bool printed = false;
    private bool inventory = false;
    public bool delay = false;
    private GUIStyle handlestyle;
    public float hide_interval = 2f;

    private void Start()
    {
        if (message == "")
        {
            this.gameObject.SetActive(false);
        }
        handlestyle = this.transform.parent.GetComponent<MainCamera>().gUISTYLE;
        if(WorldMessage)
            StartCoroutine("Off");
    }
    // Update is called once per frame
    void Update()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>().inventory_is_opened;
        if (printed)
        {
            CancelInvoke();
        }

        if(WorldMessage && !delay)
        {
            StartCoroutine("Off");
            delay = true;
        }
    }



    public void OnGUI()
    {
        GUI.color = Color.white;
        GUI.skin = transform.parent.GetComponentInChildren<GUIStyles>().skins[1];

        GUI.Box(new Rect(0f, Screen.height*0.8f, Screen.width, Screen.height*0.2f), "");
        if (!inventory)
        {
            GUI.Label(new Rect(Screen.width * 0.03f, Screen.height * 0.86f - 20f, Screen.width * 0.91f, 25f), speaker);
            GUI.Label(new Rect(Screen.width * 0.03f, Screen.height * 0.86f, Screen.width * 0.91f, 500f), message);
        }
    }

    IEnumerator Off()
    {
        yield return new WaitForSeconds(hide_interval);
        this.gameObject.SetActive(false);
    }
}
