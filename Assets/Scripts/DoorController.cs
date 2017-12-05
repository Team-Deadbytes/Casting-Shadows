using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	public bool isOpen;
	public float detectionRange;
	public GameObject player;

	public void Update()
	{
		if (Vector3.Distance(player.transform.position, transform.position) <= detectionRange)
			if (Input.GetKeyUp(KeyCode.E))
				toggleDoor();
	}

	private void toggleDoor()
	{
		float rotationAngle = isOpen ? -90.0f : 90.0f;
		transform.Rotate(0.0f, 0.0f, rotationAngle);
		isOpen = !isOpen;
	}
}
