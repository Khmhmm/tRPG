using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTiles : MonoBehaviour
{
    public List<GameObject> tileMaps;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<transform.GetChild(0).transform.childCount;i++)
        {
            tileMaps.Add(transform.GetChild(0).transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
