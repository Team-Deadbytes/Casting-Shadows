using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLightController : MonoBehaviour {
    private const string REPLACE_BULB_MESSAGE = "Press E to replace lightbulb";
    private const string FIX_LIGHT_FIXTURE = "Press E to fix light fixture and install lightbulb";
    private const string UNSCREW_LIGHTBULB_MESSAGE = "Press E to unscrew lightbulb";
    private const string SCREW_LIGHTBULB_MESSAGE = "Press E to screw in lightbulb";
    public enum LightState { NotEquipped, Equipped, BrokenBulb, BrokenGlass };

    [SerializeField]
    LightState model;
    [SerializeField]
    public bool powered, switchState;
    [SerializeField]
    public Sprite NotEquippedSprite, EquippedSprite, BrokenBulbSprite, BrokenGlassSprite;
    [SerializeField]
    public GameObject lightSource;
    [SerializeField]
    public AudioClip shatterSound, bulbScrewSound, bulbUnscrewSound;

    private SpriteRenderer sr;
    private AudioSource audioSource;
    private bool showProximityMessage;

    private void SetSprite(LightState m, bool audio = true)
    {
        model = m;
        switch (m)
        {
            case LightState.NotEquipped:
                lightSource.SetActive(false);
                powered = false;
                audioSource.clip = bulbUnscrewSound;
                sr.sprite = NotEquippedSprite;
                break;
            case LightState.Equipped:
                powered = switchState;
                lightSource.SetActive(powered);
                audioSource.clip = bulbScrewSound;
                sr.sprite = EquippedSprite;
                break;
            case LightState.BrokenBulb:
                lightSource.SetActive(false);
                powered = false;
                audioSource.clip = shatterSound;
                sr.sprite = BrokenBulbSprite;
                break;
            case LightState.BrokenGlass:
                lightSource.SetActive(false);
                powered = false;
                audioSource.clip = shatterSound;
                sr.sprite = BrokenGlassSprite;
                break;
        }
        if(audio)
            audioSource.Play();
    }

    public void toggle(bool button)
    {
        switchState = button;
        if (switchState && !powered && model == LightState.Equipped)
        {
            powered = true;
            lightSource.SetActive(true);
        }
        else if(!switchState)
        {
            powered = false;
            lightSource.SetActive(false);
        }
    }

    public void BreakGlass()
    {
        SetSprite(LightState.BrokenGlass);
    }

    public void BreakBulb()
    {
        SetSprite(LightState.BrokenBulb);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && other.isTrigger == false)
        {
            showProximityMessage = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (model == LightState.BrokenBulb)
                    SetSprite(LightState.Equipped);
                else if (model == LightState.BrokenGlass)
                    SetSprite(LightState.Equipped);
                else if (model == LightState.NotEquipped)
                    SetSprite(LightState.Equipped);
                else if (model == LightState.Equipped)
                    SetSprite(LightState.NotEquipped);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && other.isTrigger == false)
            showProximityMessage = false;
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

            if(model == LightState.BrokenBulb)
                GUI.Label(labelRect, REPLACE_BULB_MESSAGE, labelStyle);
            else if(model == LightState.BrokenGlass)
                GUI.Label(labelRect, FIX_LIGHT_FIXTURE, labelStyle);
            else if(model == LightState.Equipped)
                GUI.Label(labelRect, UNSCREW_LIGHTBULB_MESSAGE, labelStyle);
            else if(model == LightState.NotEquipped)
                GUI.Label(labelRect, SCREW_LIGHTBULB_MESSAGE, labelStyle);
        }
    }

    // Use this for initialization
    void Start () {
        showProximityMessage = false;
        sr = gameObject.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if(powered)
            SetSprite(LightState.Equipped, false);
        else
            SetSprite(model, false);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
