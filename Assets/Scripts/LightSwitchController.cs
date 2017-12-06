﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchController : MonoBehaviour
{
	private const string ON_MESSAGE  = "Press E to flip switch on";
	private const string OFF_MESSAGE = "Press E to flip switch off";

	public bool isOn;
	public Sprite onSprite;
	public Sprite offSprite;

	private SpriteRenderer spriteRenderer;
	private string proximityMessage;
	private bool showProximityMessage;

	public void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		toggleSwitch(isOn);

		spriteRenderer.sprite = isOn ? onSprite : offSprite;
		proximityMessage = isOn ? OFF_MESSAGE : ON_MESSAGE;
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
				toggleSwitch(!isOn);
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player" && other.isTrigger == false)
			showProximityMessage = false;
	}

	private void toggleSwitch(bool on)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject childObj = transform.GetChild(i).gameObject;
			childObj.gameObject.SetActive(on);
		}
		isOn = on;

		// TODO: Play audio clicks

		proximityMessage = on ? OFF_MESSAGE : ON_MESSAGE;
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
