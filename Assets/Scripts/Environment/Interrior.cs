using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrior : MonoBehaviour
{
    public GameObject father;
    public GameObject worldTiles;
    public BoxCollider2D[] collidersArr;
    public bool inside = false;
    private void Start()
    {
        InvokeRepeating("InsideChecker", 0.5f, 0.5f);
        collidersArr  = father.GetComponents<BoxCollider2D>();
        worldTiles = GameObject.Find("WorldTilemap");
    }
    private void LateUpdate()
    {
        if (inside)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }

            foreach (var col in collidersArr)
            {
                col.enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            foreach (var col in collidersArr)
            {
                col.enabled = true;
            }
        }
    }

    void InsideChecker()
    {
        worldTiles.gameObject.SetActive(!inside);
    }
}
