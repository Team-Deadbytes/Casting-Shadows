using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGhost2Door : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name.Contains("WoodenDoor"))
        {
            GameObject doorObject = other.gameObject;
            DoorController doorController = doorObject.GetComponent<DoorController>();
            if (!doorController.isOpen && doorController.isInteractable)
                doorController.toggleDoor();
        }
    }
}
