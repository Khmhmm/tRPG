using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Lighting : MonoBehaviour
{
    public float hours,minutes;
    [SerializeField]private float mp=0.5f, sun, timespeed=3f, renderDelay = 1f;
    [SerializeField]List<GameObject> tilemaps;
    public bool dynamicLightsIsOn = false;
    public Material defMaterial, normalMappedMaterial;
    public GameObject sunObject;
    private void Start()
    {
 //       tilemaps = GameObject.Find("WorldTilemap").GetComponent<WorldTiles>().tileMaps;
        InvokeRepeating("TimePlus", timespeed, timespeed);
        if (!dynamicLightsIsOn)
        {
            InvokeRepeating("StaticLight", renderDelay, renderDelay);
            foreach(var map in tilemaps)
            {
                map.GetComponent<TilemapRenderer>().material = defMaterial;
            }
            sunObject.SetActive(false);
        }
        else
        {
            InvokeRepeating("DynamicLight", renderDelay, renderDelay);
            foreach (var map in tilemaps)
            {
                map.GetComponent<TilemapRenderer>().material = normalMappedMaterial;
            }
        }
    }

    void TimePlus()
    {
            ++minutes;
        if (minutes == 60)
        {
            minutes = 0;
            ++hours;
        }

        if (hours == 24)
        {
            hours = 0f;
            mp = 0.5f;
            // добавляет тайлам красный оттенок как при рассвете/закате
            if (hours != 8 && hours != 20)
            {
                sun = mp;
            }
            else
            {
                sun = 0.75f;
            }
        }
        if (hours < 12)
        {
            mp = 0.5f + 0.5f * hours/12f;
            // добавляет тайлам красный оттенок как при рассвете/закате
            if (hours != 8 && hours != 20)
            {
                sun = mp;
            }
            else
            {
                sun = 1f;
            }
        }
        else if (hours == 12)
        {
            mp = 1f;
            // добавляет тайлам красный оттенок как при рассвете/закате
            if (hours != 8 && hours != 20)
            {
                sun = mp;
            }
            else
            {
                sun = 1f;
            }
        }
        else if (hours > 12)
        {
            mp = 1.5f - (0.5f * hours / 12f);
            // добавляет тайлам красный оттенок как при рассвете/закате
            if (hours != 8 && hours != 20)
            {
                sun = mp;
            }
            else
            {
                sun = 1f;
            }
        }
    }

    public void StaticLight()
    {
        foreach (var tilemap in tilemaps)
        {
            tilemap.GetComponent<Tilemap>().color = new Color(sun, mp, mp);
        }
    }

    public void DynamicLight()
    {
        sunObject.GetComponent<Light>().color = new Color(sun, mp, mp);
    }

    public void SetTime(float hours, float minutes)
    {
        this.hours = hours;
        this.minutes = minutes;
        this.TimePlus();
    }
}
