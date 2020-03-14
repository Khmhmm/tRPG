using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GUIStyle gUISTYLE;
    private GameObject player;
    private Lighting timeScript;
    private float hours;
    private bool pause = false;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        timeScript = GetComponentInChildren<Lighting>();
    }
    private void LateUpdate()
    {
        if (transform.position.z > player.transform.position.z)
        {
            transform.position += new Vector3(0f, 0f, player.transform.position.z - 10f);
        }
        // движение за игроком
        if (Vector3.Magnitude(player.transform.position - this.transform.position + new Vector3(0f,0f,10f)) < 20.31f)
        {
            transform.position = Vector3.Lerp(this.transform.position, player.transform.position - new Vector3(0f, 0f, 10f), 0.0062f);
        }
        else
        {
            transform.position = Vector3.Lerp(this.transform.position, player.transform.position - new Vector3(0f, 0f, 15f), 0.0162f);
        }


        if (pause)
        {
            Time.timeScale = 0.001f;
            player.GetComponentInChildren<Inventory>().inventory_is_opened = false;
        }
        else
        {
            Time.timeScale = 1f;
        }

            if (Input.GetKey(KeyCode.Escape))
            {
            pause = true;
            }


            /*
        //изменение цвета фона
        hours = timeScript.hours;
        var mp = 0.5f;
        if (hours < 12)
        {
            mp = 0.75f + +0.5f * hours / 12f;
        }
        else if (hours == 12)
        {
            mp = 1f;
        }
        else if (hours > 12)
        {
            mp = 1.5f - (0.75f * hours / 12f);
        }
        GetComponent<Camera>().backgroundColor = new Color(0f, mp/2f , mp);
        */
    }

    private void OnGUI()
    {
        if (pause)
        {
            GUI.skin = GetComponentInChildren<GUIStyles>().skins[1];
            if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, Screen.width * 0.2f, Screen.height * 0.05f), "Вернуться в игру"))
            {
                pause = false;
            }
            if(GUI.Button(new Rect(Screen.width*0.4f,Screen.height*0.6f,Screen.width*0.2f,Screen.height*0.05f),"Выйти из игры"))
            {
                Application.Quit();
            }
        }
    }
}
