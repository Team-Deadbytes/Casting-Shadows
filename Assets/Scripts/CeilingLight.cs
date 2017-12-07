using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLight : MonoBehaviour
{
	public enum LightBulbStatus
	{
		Missing,
		OK,
		Broken,
	};

	// Proximity message fields
	private const string MISSING_MESSAGE = "Hold E to screw in a light bulb to the ceiling light";
	private const string OK_MESSAGE      = "Hold E to unscrew the light bulb from the ceiling light";
	private const string BROKEN_MESSAGE  = "Hold E to unscrew the broken light bulb from the ceiling light";
	private string proximityMessage;
	private bool showProximityMessage;

	// Audio fields
	private AudioSource audioSource;
	public AudioClip insertSound;
	public AudioClip removeSound;

	// Light fields
	public LightBulbStatus lightBulbStatus;
	private Light lightComponent;
	private LightSwitch parentLightSwitch { get { return transform.parent ? transform.parent.GetComponent<LightSwitch>() : null; } }
	private bool isConnected { get { return parentLightSwitch ? parentLightSwitch.IsOn : false; } }

	// Interact fields
	public float totalActionTime;
	private bool interacting;
	private float actionProgress;
	private Vector3 playerPosition;

	public void Start()
	{
		audioSource = GetComponent<AudioSource>();
		lightComponent = GetComponent<Light>();
		SetProximityMessage();
	}

	public void Update()
	{	
		if (lightBulbStatus == LightBulbStatus.OK)
			lightComponent.enabled = parentLightSwitch ? isConnected : true;
		else
			lightComponent.enabled = false;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && other.isTrigger == false)
			showProximityMessage = true;
	}

	public void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player" && other.isTrigger == false)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				playerPosition = other.transform.position;
				StartAction();
			}
			
			if (Input.GetKeyUp(KeyCode.E))
				StopAction();

			if (interacting)
			{
				if (playerPosition == other.transform.position)
					ProgressAction();
				else
					StopAction();
			}
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player" && other.isTrigger == false)
		{
			StopAction();
			showProximityMessage = false;
		}
	}

	private void StartAction()
	{
		interacting = true;
		audioSource.clip = (lightBulbStatus == LightBulbStatus.Missing)
			? insertSound
			: removeSound;
		audioSource.Play();
	}

	private void ProgressAction()
	{
		actionProgress += 1f * Time.deltaTime;
	
		if (actionProgress >= totalActionTime)
			FinishAction();
	}

	private void FinishAction()
	{
		StopAction();
		Interact();
	}

	private void StopAction()
	{
		interacting = false;
		actionProgress = 0f;
		audioSource.Stop();
	}
	
	private void Interact()
	{
		switch (lightBulbStatus)
		{
		case LightBulbStatus.Missing:
			lightBulbStatus = LightBulbStatus.OK; // Insert light bulb
			break;
		case LightBulbStatus.OK:
		case LightBulbStatus.Broken:
			lightBulbStatus = LightBulbStatus.Missing; // Remove light bulb
			break;
		}

		SetProximityMessage();
	}

	private void SetProximityMessage()
	{
		switch (lightBulbStatus)
		{
		case LightBulbStatus.Missing:
			proximityMessage = MISSING_MESSAGE;
			break;
		case LightBulbStatus.OK:
			proximityMessage = OK_MESSAGE;
			break;
		case LightBulbStatus.Broken:
			proximityMessage = BROKEN_MESSAGE;
			break;
		}
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
