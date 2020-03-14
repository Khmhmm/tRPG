using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Effects : MonoBehaviour
{
    public float power = 1f;
    public bool delay = false;
    public int duration = 10;
    public enum Action
    {
        hpRestoring, poison
    }
    public Action itemAction;

    private void LateUpdate()
    {
        if (duration == 0)
        {
            transform.parent.GetComponent<Personality>().effects.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public IEnumerator Effecting()
    {
        yield return new WaitForSeconds(1f);
        if (itemAction == Action.hpRestoring)
        {
            
            if(transform.parent.transform.parent.GetComponentInChildren<Condition>().hp 
            <
            transform.parent.transform.parent.GetComponentInChildren<Condition>().maxhp)
            {
                transform.parent.transform.parent.GetComponentInChildren<Condition>().hp += power;
            }
            else
            {
                transform.parent.transform.parent.GetComponentInChildren<Condition>().hp = transform.parent.transform.parent.GetComponentInChildren<Condition>().maxhp;
            }
            Debug.Log("Hp incremented");
        }
        else if (itemAction == Action.poison)
        {
            transform.parent.transform.parent.GetComponentInChildren<Condition>().hp -= power;
            Debug.Log("Hp incremented");
        }
        delay = false;
        --duration;
    }

    private void OnGUI()
    {
        
    }
}
