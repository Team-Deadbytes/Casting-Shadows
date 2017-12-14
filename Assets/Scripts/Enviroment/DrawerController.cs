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

    void Start ()
    {
        progressBar = GameObject.Find("/Canvas/ChangeLightProgressBar");
        animator = GetComponent<Animator>();
        progressBarImg = progressBar.GetComponent<Image>();
        timedMessage = GetComponent<TimedMessage>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && playersInventory == null)
            playersInventory = other.GetComponent<Inventory>();
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && !other.isTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerPosition = other.transform.position;
                StartAction();
            }

            if (interacting)
            {
                if (Input.GetKeyUp(KeyCode.E))
                {
                    StopAction();
                    animator.SetTrigger("StopOpening");
                }
                
                if (playerPosition == other.transform.position)
                    ProgressAction();
                else
                {
                    StopAction();
                    animator.SetTrigger("StopOpening");
                }
            }
        }
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
            stringBuilder.Append(".");
            timedMessage.Message = stringBuilder.ToString();
            timedMessage.Show();
            for (int i = 0; i < LightBulbCount; i++)
                playersInventory.AddLightBulb(new LightBulb());
            LightBulbCount = 0;
        }
        else
        {
            timedMessage.Message = "There is nothing in these drawers.";
            timedMessage.Show();
        }
    }
}
