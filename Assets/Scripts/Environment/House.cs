using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public GameObject door, interrior, exitDoor;
    public float doorCoordX, doorCoordY;
    private void Start()
    {
        door = Instantiate(door, 
                transform.position + new Vector3(doorCoordX * this.transform.localScale.x,doorCoordY *this.transform.localScale.y,10f), Quaternion.Euler(0f, 0f, 0f));

        door.GetComponent<Door>().father = gameObject;
        interrior = Instantiate(interrior, 
                transform.position + new Vector3(0f, 0f, -30f), transform.rotation);

        interrior.GetComponent<Interrior>().father = gameObject;
        exitDoor = interrior.transform.GetChild(0).gameObject;
    }

    private void LateUpdate()
    {
        door.SetActive(!exitDoor.activeSelf);
        GetComponent<SpriteRenderer>().enabled = !exitDoor.activeSelf;
    }
}
