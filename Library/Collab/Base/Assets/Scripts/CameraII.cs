using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraII : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        this.transform.position.x = player.transform.position.x;
        this.transform.position.y = player.transform.position.y;
    }
}
