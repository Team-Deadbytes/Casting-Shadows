using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashController : MonoBehaviour
{
    // how much stamina player has at given moment
    float currentStamina;

    // stamina: max player stamina
    public float stamina, regenRate, decreaseRate;
    bool canDash, dashing;
    GameObject staminaBar;
    Image staminaImage;

    // Use this for initialization
    void Start()
    {
        if(stamina == 0)
            stamina = 100;
        if (regenRate == 0)
            regenRate = 20;
        if (decreaseRate == 0)
            decreaseRate = 200;
        canDash = true;
        currentStamina = stamina;
        staminaBar = GameObject.Find("/Canvas/StaminaCircle");
        staminaImage = staminaBar.GetComponent<Image>();
        staminaImage.fillAmount = 100;
    }

    // Update is called once per frame
    void Update()
    {
        // if player is dashing decrease his stamina
        if (dashing)
            currentStamina -= decreaseRate * Time.deltaTime;

        // if stamina reaches 0 then player cant dash
        if (currentStamina <= 0)
            stopDash();

        // if player cant dash then he regenerates stamina
        if (!canDash && currentStamina < stamina)
            currentStamina += regenRate * Time.deltaTime;

        // when stamina is completely regenerated then player can dash again
        if (currentStamina >= stamina)
            canDash = true;

        //Debug.Log((currentStamina / stamina) * 100);
        staminaImage.fillAmount = currentStamina / stamina; // update stamina circle
    }

    // when player tries to dash, returns true if he can dash and sets dashing to true
    public bool dash()
    {
        if (canDash)
            dashing = true;
        return canDash;
    }

    // if player stops dashing before finishing his stamina
    public void stopDash()
    {
        dashing = false;
        canDash = false;
    }
}
