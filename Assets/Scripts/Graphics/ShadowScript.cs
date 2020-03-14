using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    private GameObject timeObject;
    private Lighting timeScript;
    // Start is called before the first frame update
    void Start()
    {
        timeObject = GameObject.Find("TimeCounter");
        timeScript = timeObject.GetComponent<Lighting>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // убрать поворот за игроком
        transform.lossyScale.Set(Mathf.Abs(this.transform.lossyScale.x), this.transform.lossyScale.y, 1f);
        if (timeScript.hours < 12f && timeScript.hours>7f)
        {
            transform.localPosition = new Vector3(-0.082f + (timeScript.hours - 8f) * 0.005f + timeScript.minutes * (timeScript.hours - 7f) * 0.005f/60f, -0.1227f, 2f);
            transform.localScale = new Vector3(0.4736f - (timeScript.hours-8f)*0.0455f - timeScript.minutes * (timeScript.hours - 7f) * 0.0455f/60f, 0.217354f, 1f);
        }
        else if(timeScript.hours>12f && timeScript.hours < 21f)
        {
            transform.localPosition = new Vector3(0.082f + (timeScript.hours - 12f) * 0.005f + timeScript.minutes * (timeScript.hours - 12f) * 0.005f/60f, -0.131f, 2f);
            transform.localScale = new Vector3(0.20096f + (timeScript.hours - 12f) * 0.0455f + timeScript.minutes * (timeScript.hours - 12f) * 0.0455f/60f, 0.217354f, 1f);
        }
        else
        {
            transform.localPosition = new Vector3(0.0053f, -0.128f, 2f);
            transform.localScale = new Vector3(0.20096f, 0.20096f, 1f);
        }
    }
}
