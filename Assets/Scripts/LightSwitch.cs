using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
	// Proximity message fields
	private const string ON_MESSAGE  = "Press E to flip switch on";
	private const string OFF_MESSAGE = "Press E to flip switch off";
	private string proximityMessage;
	private bool showProximityMessage;

	// Sprite fields
	private SpriteRenderer spriteRenderer;
	public Sprite OnSprite;
	public Sprite OffSprite;

	// Object fields
	public bool IsOn;

	public void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		SetSprite();
		SetProximityMessage();
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
				Interact();
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player" && other.isTrigger == false)
			showProximityMessage = false;
	}

	private void Interact()
	{
		IsOn = !IsOn;

		// TODO: Play audio clicks

		SetSprite();
		SetProximityMessage();
	}

	private void SetProximityMessage()
	{
		proximityMessage = IsOn ? OFF_MESSAGE : ON_MESSAGE;		
	}

	private void SetSprite()
	{
		spriteRenderer.sprite = IsOn ? OnSprite : OffSprite;
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
