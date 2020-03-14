using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueableNPC : MonoBehaviour
{
    protected bool dialogue = false, firstReplics = true, answering = false;
    public GameObject dialogueWindow;
    public List<Question> questions;
    public string exit_dialogue = "Ну, я пошёл";
    public string speaker, start_replics;
    protected int number_of_question;
    protected int index = 0;
    protected bool delay = false;
    protected Condition condScript;

    protected void Awake()
    {
        if (dialogueWindow == null)
        {
            dialogueWindow = GameObject.FindWithTag("MainCamera").transform.GetChild(0).gameObject;
        }
       condScript =  GameObject.Find("Condition").gameObject.GetComponent<Condition>();
    }

    protected void LateUpdate()
    {
        if(delay && Input.GetKey(KeyCode.Space))
        {
            System.Threading.Thread.Sleep(150);
            delay = false;
            if (index < questions[number_of_question].answer.dialogues.Count - 1)
                ++index;
            else
            {
                index = 0;
                StopCoroutine("Delay");
                firstReplics = true;
            }
        }
    }

    protected void OnGUI()
    {
        GUI.skin = dialogueWindow.transform.parent.GetComponentInChildren<GUIStyles>().skins[1];
        if (dialogue)
        {
            if (firstReplics)
            {
                dialogueWindow.GetComponent<HandleScript>().speaker = speaker;
                dialogueWindow.GetComponent<HandleScript>().message = start_replics;
                for (int i = 0; i < questions.Count; i++)
                {
                    if (GUI.Button(new Rect(Screen.width * 0.7f, Screen.height * 0.3f + i *20f, Screen.width * 0.25f, 20f), questions[i].textOfQuestion))
                    {
                        number_of_question = i;
                        firstReplics = !firstReplics;
                    }
                }
                if (GUI.Button(new Rect(Screen.width * 0.7f, Screen.height * 0.7f + 20f * questions.Count, Screen.width * 0.25f, 20f), exit_dialogue))
                {
                    dialogue = false;
                    dialogueWindow.GetComponent<HandleScript>().WorldMessage = true;
                    dialogueWindow.GetComponent<HandleScript>().delay = false;
                    condScript.current_status = Condition.Status.freewalking;
                }
            }
            else
            {
                if (!delay)
                {
                    StartCoroutine("Delay");
                    delay = true;
                }
                if (RunDialogue(questions[number_of_question].answer).speaker != "$player")
                    dialogueWindow.GetComponent<HandleScript>().speaker = RunDialogue(questions[number_of_question].answer).speaker;
                else {
                    dialogueWindow.GetComponent<HandleScript>().speaker = GameObject.Find("Personality").GetComponent<Personality>().Character_name;
                }
                dialogueWindow.GetComponent<HandleScript>().message = RunDialogue(questions[number_of_question].answer).text;
            }
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E)&&!dialogue && condScript.current_status!=Condition.Status.dialogue)
            {
                dialogue = true;
                dialogueWindow.SetActive(true);
                dialogueWindow.GetComponent<HandleScript>().WorldMessage = false;
                condScript.current_status = Condition.Status.dialogue;
            }
            
        }
    }

    protected Dialogue RunDialogue(OneDialogue oDialogues)
    {
        return oDialogues.dialogues[index];
    }

    protected IEnumerator Delay()
    {
            yield return new WaitForSeconds(2f + questions[number_of_question].answer.dialogues[index].text.Length/50f);
            delay = false;
            if (index < questions[number_of_question].answer.dialogues.Count - 1)
                ++index;
            else
            {
                index = 0;
                firstReplics = true;
            } 
    }
}

[System.Serializable]public class Question
{
    public string textOfQuestion;
    public OneDialogue answer;
}