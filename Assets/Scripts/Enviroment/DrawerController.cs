using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class DrawerController : MonoBehaviour
{
    private SpriteRenderer sr;

    public int LightBulbCount;

    private Inventory playersInventory;

    private float totalActionTime = 1.5f;
    private bool interacting;
    private float actionProgress;
    Vector3 playerPosition;

    private Animator animator;
    private GameObject progressBar;
    private Image progressBarImg;

    private TimedMessage timedMessage;

    private bool playerNearDrawer;

    private GameObject player;

    void Start ()
    {
        progressBar = GameObject.Find("/Canvas/ChangeLightProgressBar");
        animator = GetComponent<Animator>();
        progressBarImg = progressBar.GetComponent<Image>();
        timedMessage = GetComponent<TimedMessage>();
        player = GameObject.Find("Player");
        playersInventory = player.GetComponent<Inventory>();
    }

    public void Update()
    {
        if (playerNearDrawer)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerPosition = player.transform.position;
                StartAction();
            }

            if (interacting)
            {
                if (Input.GetKeyUp(KeyCode.E))
                {
                    StopAction();
                    animator.SetTrigger("StopOpening");
                }
                
                if (playerPosition == player.transform.position)
                    ProgressAction();
                else
                {
                    StopAction();
                    animator.SetTrigger("StopOpening");
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !other.isTrigger)
            playerNearDrawer = true;
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && !other.isTrigger)
            playerNearDrawer = false;
    }

    private void StartAction()
    {
        interacting = true;
        progressBarImg.enabled = true;
        animator.SetTrigger("Open");
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
        SearchDrawers();
    }

    private void StopAction()
    {
        interacting = false;
        actionProgress = 0;
        progressBarImg.enabled = false;
    }

    private void SearchDrawers()
    {
        if (LightBulbCount > 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("You found " + LightBulbCount + " light bulb");
            if (LightBulbCount > 1)
                stringBuilder.Append("s");
            timedMessage.Message = stringBuilder.ToString();
            timedMessage.Show();
            for (int i = 0; i < LightBulbCount; i++)
                playersInventory.AddLightBulb(new LightBulb());
            LightBulbCount = 0;
        }
        else
        {
            timedMessage.Message = "There is nothing in these drawers";
            timedMessage.Show();
        }
    }
}
