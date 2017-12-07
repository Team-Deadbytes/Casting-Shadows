using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
	// Proximity message fields
	private const string ON_MESSAGE  = "Press E to flip switch on";
	private const string OFF_MESSAGE = "Press E to flip switch off";

	// Sprite fields
	private SpriteRenderer spriteRenderer;
	public Sprite OnSprite;
	public Sprite OffSprite;

	private ProximityMessage proximityMessage;

	// Object fields
	public bool IsOn;

	public void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		proximityMessage = GetComponent<ProximityMessage>();
		SetSprite();
		SetProximityMessage();
	}

	public void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player" && other.isTrigger == false)
			if (Input.GetKeyDown(KeyCode.E))
				Interact();
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
		proximityMessage.Message = IsOn ? OFF_MESSAGE : ON_MESSAGE;		
	}

	private void SetSprite()
	{
		spriteRenderer.sprite = IsOn ? OnSprite : OffSprite;
	}
}
