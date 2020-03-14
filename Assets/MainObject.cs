using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObject : MonoBehaviour
{
    public GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
}
