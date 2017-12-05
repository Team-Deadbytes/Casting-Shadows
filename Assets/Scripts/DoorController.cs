using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	public bool isOpen;

	public void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
			if (Input.GetKeyDown(KeyCode.E))
				toggleDoor();
	}

	private void toggleDoor()
	{
		float rotationAngle = isOpen ? -90.0f : 90.0f;
		transform.Rotate(0.0f, 0.0f, rotationAngle);
		isOpen = !isOpen;
	}
}
