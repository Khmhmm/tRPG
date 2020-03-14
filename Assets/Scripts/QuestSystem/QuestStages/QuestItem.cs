using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : Unusable
{
    public bool done = false, added=false;
    public GameObject fatherQuest;

    // Update is called once per frame
    new public void FixedUpdate()
    {
        base.FixedUpdate();
        if (collected)
        {
            done = true;
        }
    }
}
