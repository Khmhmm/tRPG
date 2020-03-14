using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : DialogueableNPC
{
    public bool done = false, added = false,deleted = false;
    public Question questQuestion;
    public GameObject fatherQuest;

    private void Start()
    {
        done = false;
        added = false;
        deleted = false;
        questions.Add(questQuestion);
        questQuestion.textOfQuestion += "[!!]";
    }

    /*
     * Самое главное, что нужно усвоить.
     * В инспекторе должен быть хотя бы(!) один диалог не квестового направления.
     * Иначе работает все не очень гладко.
     * Даже после фикса могут быть проблемы.
     */

    new private void LateUpdate()
    {
        base.LateUpdate();
        if(done && !deleted && fatherQuest.GetComponent<Quest>().done)
        {
            questions.Remove(questQuestion);
            deleted = true;
        }
    }

    new void OnGUI()
    {
        base.OnGUI();
        if (dialogue)
        {
            if (firstReplics)
            {
                if (done!=true)
                {
                    if (number_of_question == questions.Count - 1 )
                    {
                        done = true;
                    }
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
                else
                {
                    dialogueWindow.GetComponent<HandleScript>().speaker = GameObject.Find("Personality").GetComponent<Personality>().Character_name;
                }
                dialogueWindow.GetComponent<HandleScript>().message = RunDialogue(questions[number_of_question].answer).text;
            }
        }
    }
}
