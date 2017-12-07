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

	// Audio fields
	private AudioSource audioSource;
	public AudioClip insertSound;
	public AudioClip removeSound;
	public AudioClip breakSound;

	// Light fields
	public LightBulbStatus lightBulbStatus;
	private Light lightComponent;
	private LightSwitch parentLightSwitch { get { return transform.parent ? transform.parent.GetComponent<LightSwitch>() : null; } }
	private bool isConnected { get { return parentLightSwitch ? parentLightSwitch.IsOn : false; } }

	// Interact fields
	private float totalActionTime { get { return (lightBulbStatus == LightBulbStatus.Missing) ? insertSound.length : removeSound.length; } }
	private bool interacting;
	private float actionProgress;
	private Vector3 playerPosition;

	private ProximityMessage proximityMessage;

	public void Start()
	{
		audioSource = GetComponent<AudioSource>();
		lightComponent = GetComponentInChildren<Light>();
		proximityMessage = GetComponent<ProximityMessage>();
		SetProximityMessage();
	}

	public void Update()
	{	
		if (lightBulbStatus == LightBulbStatus.OK)
			lightComponent.enabled = parentLightSwitch ? isConnected : true;
		else
			lightComponent.enabled = false;
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

	private void InsertLightBulb()
	{
		lightBulbStatus = LightBulbStatus.OK;
		transform.GetComponentInChildren<CeilingLightChild>().Renew();
		SetProximityMessage();
	}

	private void RemoveLightBulb()
	{
		lightBulbStatus = LightBulbStatus.Missing;
		SetProximityMessage();
	}

	public void BreakLightBulb()
	{
		audioSource.clip = breakSound;
		audioSource.Play();
		lightBulbStatus = LightBulbStatus.Broken;
		SetProximityMessage();
	}
	
	private void Interact()
	{
		switch (lightBulbStatus)
		{
		case LightBulbStatus.Missing:
			InsertLightBulb();
			break;
		case LightBulbStatus.OK:
		case LightBulbStatus.Broken:
			RemoveLightBulb();
			break;
		}
	}

	private void SetProximityMessage()
	{
		switch (lightBulbStatus)
		{
		case LightBulbStatus.Missing:
			proximityMessage.Message = MISSING_MESSAGE;
			break;
		case LightBulbStatus.OK:
			proximityMessage.Message = OK_MESSAGE;
			break;
		case LightBulbStatus.Broken:
			proximityMessage.Message = BROKEN_MESSAGE;
			break;
		}
	}
}
