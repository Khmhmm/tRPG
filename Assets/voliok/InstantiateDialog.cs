using UnityEngine;
using System.Collections;

public class InstantiateDialog : MonoBehaviour
{
    public TextAsset ta;
    public Dialog dialog;
    public int currentNode;
    public bool ShowDialogue;

    public GUISkin skin;

    void Start()
    {
        dialog = Dialog.Load(ta);
        skin = Resources.Load("Skin") as GUISkin;
    }

    void Update()
    {

    }

    void OnGUI()
    {
        GUI.skin = skin;
        if (ShowDialogue)
        {
            GUI.Box(new Rect(Screen.width / 2 - 300, Screen.height - 300, 600, 250), "");
            GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height - 280, 500, 90), dialog.nodes[currentNode].NpcText);
            for (int i = 0; i < dialog.nodes[currentNode].answers.Length; i++)
            {
                if (GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height - 200 + 25 * i, 500, 25), dialog.nodes[currentNode].answers[i].text, skin.label))
                {
                    if (dialog.nodes[currentNode].answers[i].end == "true")
                    {
                        ShowDialogue = false;
                    }
                    currentNode = dialog.nodes[currentNode].answers[i].nextNode;
                }
            }
        }
    }
}