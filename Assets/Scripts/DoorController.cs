using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	private const string CLOSE_MESSAGE = "Press E to close door";
	private const string OPEN_MESSAGE  = "Press E to open door";

	public AudioClip openSound;
	public AudioClip closeSound;
	public bool isOpen;

	private AudioSource audioSource;
	private string proximityMessage = OPEN_MESSAGE;
	private bool showProximityMessage;

	public void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && other.isTrigger == false)
			showProximityMessage = true;
	}

	public void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player" && other.isTrigger == false)
			if (Input.GetKeyDown(KeyCode.E))
				toggleDoor();
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player" && other.isTrigger == false)
			showProximityMessage = false;
	}

	public void toggleDoor()
	{
		float rotationAngle = isOpen ? -90.0f : 90.0f;
		transform.Rotate(0.0f, 0.0f, rotationAngle);
		isOpen = !isOpen;

		audioSource.clip = isOpen ? openSound : closeSound;
		audioSource.Play();

		proximityMessage = !isOpen ? OPEN_MESSAGE : CLOSE_MESSAGE;
	}

	private void OnGUI()
	{
		if (showProximityMessage)
		{
			GUIStyle labelStyle = GUI.skin.GetStyle("Label");
			labelStyle.alignment = TextAnchor.UpperCenter;

			float labelWidth = 300f;
			float labelHeight = 50f;
			float labelX = (Screen.width - labelWidth) / 2;
			float labelY = Screen.height - 100;
			Rect labelRect = new Rect(labelX, labelY, labelWidth, labelHeight);

			GUI.Label(labelRect, proximityMessage, labelStyle);
		}
	}
}
