using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	private const string CLOSE_MESSAGE = "Press E to close door";
	private const string OPEN_MESSAGE  = "Press E to open door";

	public bool isOpen;
	public bool isInteractable;

	public AudioClip openSound;
	public AudioClip closeSound;
	private AudioSource audioSource;
	private ProximityMessage proximityMessage;

	public void Start()
	{
		audioSource = GetComponent<AudioSource>();
		proximityMessage = GetComponent<ProximityMessage>();
		setProximityMessage();		
	}

	private void setProximityMessage()
	{
		proximityMessage.Message = isInteractable
			? isOpen ? CLOSE_MESSAGE : OPEN_MESSAGE
			: string.Empty;
	}

	public void OnTriggerStay2D(Collider2D other)
	{
		if (isInteractable)
			if (other.tag == "Player" && other.isTrigger == false)
				if (Input.GetKeyDown(KeyCode.E))
					toggleDoor();
	}

	public void toggleDoor()
	{
		float rotationAngle = isOpen ? -90.0f : 90.0f;
		transform.Rotate(0.0f, 0.0f, rotationAngle);
		isOpen = !isOpen;

		audioSource.clip = isOpen ? openSound : closeSound;
		audioSource.Play();

		setProximityMessage();
	}
}
