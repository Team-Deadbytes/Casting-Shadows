using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    private const string SEARCH_MESSAGE  = "Press E to search the drawers";
    private const string CLOSE_MESSAGE = "Press E to close the drawers";

    private SpriteRenderer sr;
    public Sprite closedSprite;
    public Sprite open1Sprite;
    // public Sprite open2Sprite;

    public bool open1;
    // public bool open2;

    public int LightBulbCount;

    private ProximityMessage proximityMessage;
    private Inventory playersInventory;

    void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        proximityMessage = GetComponent<ProximityMessage>();
        SetSprite();
        SetProximityMessage();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && playersInventory == null)
            playersInventory = other.GetComponent<Inventory>();
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && !other.isTrigger)
            if (Input.GetKeyDown(KeyCode.E))
                ToggleDrawer();
    }

    public void ToggleDrawer()
    {
        open1 = !open1;
        if (open1)
        {
            if (LightBulbCount > 0)
            {
                // TODO: Display message "You found X light bulbs" etc.
                Debug.Log("You found " + LightBulbCount + " light bulbs");
                for (int i = 0; i < LightBulbCount; i++)
                    playersInventory.AddLightBulb(new LightBulb());
                LightBulbCount = 0;
            }
            else
            {
                // TODO: Display message "There is nothing in these drawers" etc.
                Debug.Log("There is nothing in these drawers");
            }
        }
        SetSprite();
        SetProximityMessage();
    }

    private void SetSprite()
    {
        // If both open1 and open2 are true, then open1Sprite is rendered.
        // Perhaps add a sprite with both drawers open?
        if (open1)
            sr.sprite = open1Sprite;
        // else if (open2)
        //     sr.sprite = open2Sprite;
        else
            sr.sprite = closedSprite;
    }

    private void SetProximityMessage()
    {
        // if (!open1 && !open2)
        if (!open1)
            proximityMessage.Message = SEARCH_MESSAGE;
        else
            proximityMessage.Message = CLOSE_MESSAGE;
    }
}
