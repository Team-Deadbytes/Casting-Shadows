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
        stamina = 100;
        regenRate = 1;
        decreaseRate = 5;
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
            currentStamina -= decreaseRate;

        // if stamina reaches 0 then player cant dash
        if (currentStamina == 0)
            stopDash();

        // if player cant dash then he regenerates stamina
        if (!canDash && currentStamina < stamina)
            currentStamina += regenRate;

        // when stamina is completely regenerated then player can dash again
        if (currentStamina >= stamina)
            canDash = true;

        Debug.Log((currentStamina / stamina) * 100);
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
