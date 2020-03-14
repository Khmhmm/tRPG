using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Перестань дружить со мною
 * Я тебе не подхожу
 * Потому что мы с тобою
 * Ходим к одному врачу
 * Поцелуй меня покрепче
 * Мы по космосу летим
 * Не влюбляйся в меня сильно
 * Это все флуоксетин
 * https://www.youtube.com/watch?v=69MwjnfJu0E
 */

public class RandomNPCGenerator : MonoBehaviour
{
    public List<GameObject> heads, bodies, shirts, legses;
    private GameObject head,body,shirt,legs;
    void Start()
    {

        var rand = new System.Random();
        head = heads[rand.Next() % heads.Count];
        body = bodies[rand.Next() % bodies.Count];
        shirt = shirts[rand.Next() % shirts.Count];
        legs = legses[rand.Next() % legses.Count];
        Instantiate(head,this.transform);
        head.transform.localPosition=new Vector3(0.054f,0.2603f,-3f);

        Instantiate(body,this.transform);
        body.transform.localPosition=new Vector3(-0.007f,-0.0099f,-2f);

        Instantiate(shirt,this.transform);
        shirt.transform.localPosition=new Vector3(-0.007f,0.02f,-3f);

        Instantiate(legs,this.transform);
        legs.transform.localPosition=new Vector3(0.008f,-0.2047f,-3f);

    }
}
