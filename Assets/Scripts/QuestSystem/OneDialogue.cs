using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]public class OneDialogue : MonoBehaviour
{    
        public List<Dialogue> dialogues;
}

[System.Serializable]public class Dialogue
{
    public string speaker, text;
}
