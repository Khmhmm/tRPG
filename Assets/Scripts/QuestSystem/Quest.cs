using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public string questName = " ";
    public int currentStage = 0;
    public int exp=100;
    public bool done = false;
    protected bool delay = false;
    protected bool spawned = false;
    public List<GameObject> questStages;
    protected GUIStyles stylesScript;
    public AudioSource audioSource;

    protected enum StageType
    {
        npc,item
    }
    protected StageType currentStageType;
    protected void Start()
    {
        currentStage = 0;
        transform.parent = GameObject.Find("QuestSystem").transform;
        transform.parent.GetComponent<QuestSystem>().questList.Add(this.gameObject);
        InvokeRepeating("CheckStageType", 1.5f + Time.deltaTime, 1.5f + Time.deltaTime);
        stylesScript = GameObject.FindGameObjectWithTag("MainCamera").transform.Find("GUIStyles").GetComponent<GUIStyles>();
    }

    protected void LateUpdate()
    {
        if (currentStage == questStages.Count)
        {
            done = true;
            if (done && !delay)
            {
                GetComponent<AudioSource>().enabled = true;
                Debug.Log("Clip has been started");
                CancelInvoke("CheckStageType");
                StartCoroutine("Delay");
                delay = true;
            }
        }
        else
        {
            if (currentStageType == StageType.npc)
            {
                if (!spawned)
                {
                    spawned = true;
                    questStages[currentStage] = Instantiate(questStages[currentStage]);
                    questStages[currentStage].GetComponent<QuestNPC>().fatherQuest = this.gameObject;
                }

                //короче. 
                //Когда помимо квестовой реплики ничо нет выпрыгивает эксцепшн. Хз, что с ним делать и как его чинить. Такие дела. (Вроде как проблемы с индексацией в RunDialogue)
                try
                {
                    if (questStages[currentStage].gameObject.GetComponent<QuestNPC>() == null)
                    {
                        throw new System.Exception("qNPC component is null");
                    }
                    if (questStages[currentStage].GetComponent<QuestNPC>().done == true)
                    {
                        if (questStages[currentStage].GetComponent<QuestNPC>().added == false)
                        {
                            ++currentStage;
                            spawned = false;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.Log("Something goes wrong. I cant find QuestNPC script." + e);
                }
            }
            else if(currentStageType == StageType.item)
            {
                if (!spawned)
                {
                    spawned = true;
                    questStages[currentStage] = Instantiate(questStages[currentStage]);
                    questStages[currentStage].GetComponent<QuestItem>().fatherQuest = this.gameObject;
                }
                if (questStages[currentStage].GetComponent<QuestItem>().done == true)
                {
                    if (questStages[currentStage].GetComponent<QuestItem>().added == false)
                    {
                        ++currentStage;
                        spawned = false;
                    }
                }
            }
        }
    }

    protected void OnGUI()
    {
        GUI.skin = stylesScript.skins[1];
        if (done)
        {
            GUI.Label(new Rect(Screen.width * 0.4f, Screen.height * 0.1f, Screen.width * 0.2f, 40f), "Квест " + questName + " выполнен");
        }
    }

    // этот метод проверяет тип стадии квеста каждую секунду
    protected void CheckStageType()
    {
        try
        {
            if (questStages[currentStage].GetComponent<QuestNPC>() != null)
            {
                currentStageType = StageType.npc;
            }
            else if (questStages[currentStage].GetComponent<QuestItem>() != null)
            {
                currentStageType = StageType.item;
            }
        }
        catch(System.ArgumentOutOfRangeException e)
        {
            Debug.Log(e);
            CancelInvoke("CheckStageType");
        }
        Debug.Log("//  //Stage checked");
    }

    protected IEnumerator Delay()
    {
        yield return new WaitForSeconds(5f);
        GameObject.FindWithTag("Player").GetComponentInChildren<Condition>().exp += exp;
        // добавить exp, вывести запись о выполнении квеста
        Destroy(this.gameObject);
    }
}
