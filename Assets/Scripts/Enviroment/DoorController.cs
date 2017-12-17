using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	private const string CLOSE_MESSAGE = "Press E to close door";
	private const string OPEN_MESSAGE  = "Press E to open door";

	public bool isOpen;
	public bool isInteractable;
	public bool isLocked;
	private bool playerNearDoor;

	public AudioClip openSound;
	public AudioClip closeSound;
	private AudioSource audioSource;
	private ProximityMessage proximityMessage;
	private TimedMessage timedMessage;

	public void Start()
	{
		audioSource = GetComponent<AudioSource>();
		proximityMessage = GetComponent<ProximityMessage>();
		timedMessage = GetComponent<TimedMessage>();
		setProximityMessage();		
	}

	public void Update()
	{
		if (playerNearDoor && isInteractable && Input.GetKeyDown(KeyCode.E))
		{
			if (!isLocked)
				toggleDoor();
			else
				timedMessage.Show();
		}
	}

	private void setProximityMessage()
	{
		proximityMessage.Message = isInteractable
			? isOpen ? CLOSE_MESSAGE : OPEN_MESSAGE
			: string.Empty;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && other.isTrigger == false)
			playerNearDoor = true;
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player" && other.isTrigger == false)
			playerNearDoor = false;
	}

	public void toggleDoor()
	{
		if (!isLocked)
		{
			float rotationAngle = isOpen ? -90.0f : 90.0f;
			transform.Rotate(0.0f, 0.0f, rotationAngle);
			isOpen = !isOpen;

			audioSource.clip = isOpen ? openSound : closeSound;
			audioSource.Play();

			setProximityMessage();
		}
	}
}
