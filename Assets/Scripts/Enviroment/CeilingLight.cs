using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CeilingLight : MonoBehaviour
{
	private const string INSERT_MESSAGE = "Hold E to insert a light bulb to the light";
	private const string REMOVE_MESSAGE = "Hold E to remove the light bulb from the light";

	private AudioSource audioSource;
	public AudioClip insertSound;
	public AudioClip removeSound;
	public AudioClip breakSound;

	public bool ConnectedToSwitch { get { return (transform.parent && transform.parent.tag == "LightSwitch"); } }
	public bool SwitchIsOn { get { return parentLightSwitch ? parentLightSwitch.IsOn : false; } }
	public bool LightIsOn
	{
		get
		{
			if (ConnectedToSwitch)
				return SwitchIsOn && lightBulb != null;
			else
				return lightBulb != null;
		}
	}
	private Light lightComponent;
	private CeilingLightChild child;
    private LightSwitch parentLightSwitch { get { return ConnectedToSwitch ? transform.parent.GetComponent<LightSwitch>() : null; } }
	private LightBulb lightBulb;
	public LightBulb LightBulb { get { return lightBulb; } }
	public bool StartWithLightBulb;
	public float StartLightBulbLifetime;
	
	private bool isFlickering;
	public bool IsFlickering { get { return isFlickering; } }
	public float MinFlickerTime;
	public float MaxFlickerTime;

	private float totalActionTime { get { return (lightBulb != null) ? removeSound.length : insertSound.length; } }
	private bool interacting;
	private float actionProgress;
	private Vector3 playerPosition;

    private bool monsterNear;
    private float monsterGracePeriod;
    private float originalIntensity;

	private ProximityMessage proximityMessage;

	private Inventory playersInventory;

    private GameObject progressBar;
    private Image progressBarImg;
    
    private SanitySystem playersSanitySystem;

	private TimedMessage timedMessage;

	public void Start()
	{
		audioSource = GetComponent<AudioSource>();
		lightComponent = GetComponentInChildren<Light>();
		proximityMessage = GetComponent<ProximityMessage>();
        monsterGracePeriod = 0.0f;
        monsterNear = false;
        originalIntensity = lightComponent.intensity;
		child = transform.GetComponentInChildren<CeilingLightChild>();
		playersInventory = GameObject.Find("Player").GetComponent<Inventory>();
		playersSanitySystem = GameObject.Find("Player").transform.Find("Player top light").GetComponent<SanitySystem>();

		timedMessage = GetComponent<TimedMessage>();

		if (StartWithLightBulb)
			SpawnLightBulb(StartLightBulbLifetime);

		SetProximityMessage();

        progressBar = GameObject.Find("/Canvas/ChangeLightProgressBar");
        progressBarImg = progressBar.GetComponent<Image>();
        progressBarImg.fillAmount = 0;
    }

	public void Update()
    {
		if (!IsFlickering)
			lightComponent.enabled = LightIsOn;

        if ((monsterNear || monsterGracePeriod > 0.0f) && lightComponent.isActiveAndEnabled && lightComponent.intensity > 0.0f)
        {
            monsterGracePeriod -= Time.deltaTime;
            if (monsterGracePeriod < 0.0f)
                monsterGracePeriod = 0.0f;
            lightComponent.intensity = lightComponent.intensity - (6.0f * Time.deltaTime);
            if (lightComponent.intensity <= 0.0f)
            {
                lightComponent.intensity = originalIntensity;
				BreakLightBulb();
                monsterNear = false;
                monsterGracePeriod = 0.0f;
            }
        }
    }

	public void Flicker()
	{
		isFlickering = true;
		StartCoroutine(flicker());
	}

	private IEnumerator flicker()
	{
		while (LightIsOn && child.ShouldFlicker())
		{
			lightComponent.enabled = false;
			yield return new WaitForSeconds(Random.Range(MinFlickerTime, MaxFlickerTime));
			lightComponent.enabled = LightIsOn;
			yield return new WaitForSeconds(Random.Range(MinFlickerTime, MaxFlickerTime));
		}
		isFlickering = false;
	}

	public void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player" && !other.isTrigger)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				if (lightBulb == null && playersInventory.LightBulbs.Count <= 0)
				{
					timedMessage.Message = "I don't have any light bulbs.";
					timedMessage.Show();
				}
				else
				{
					playerPosition = other.transform.position;
					StartAction();
				}
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
		audioSource.clip = (lightBulb != null)
			? removeSound
			: insertSound;
		audioSource.Play();
        progressBarImg.enabled = true;
    }

	public void MonsterProwing(bool state)
    {
        monsterNear = state;
        if (!state)
            monsterGracePeriod = 1.7f;
    }

	private void ProgressAction()
	{
		actionProgress += Time.deltaTime;

		progressBarImg.fillAmount = actionProgress / totalActionTime;
	
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
        progressBarImg.enabled = false;
    }

	private void InsertLightBulb()
	{
		lightBulb = playersInventory.RemoveLightBulb();
		if (child.PlayerUnderLight)
			playersSanitySystem.IncrementSafeZone();
		SetProximityMessage();
	}

	private void SpawnLightBulb(float lifetime)
	{
		lightBulb = new LightBulb(lifetime);
		if (child.PlayerUnderLight)
			playersSanitySystem.IncrementSafeZone();
		SetProximityMessage();
	}

	private void RemoveLightBulb()
	{
		if (child.PlayerUnderLight)
		{
			playersInventory.AddLightBulb(lightBulb);
			playersSanitySystem.DecrementSafeZone();
		}
		lightBulb = null;
		SetProximityMessage();
	}

	public void BreakLightBulb()
	{
		audioSource.clip = breakSound;
		audioSource.Play();
		lightBulb = null;
		if (child.PlayerUnderLight)
			playersSanitySystem.DecrementSafeZone();
		SetProximityMessage();
	}
	
	private void Interact()
	{
		if (lightBulb != null)
			RemoveLightBulb();
		else
			InsertLightBulb();
	}

	private void SetProximityMessage()
	{
		proximityMessage.Message = (lightBulb != null) ? REMOVE_MESSAGE : INSERT_MESSAGE;
	}
}
