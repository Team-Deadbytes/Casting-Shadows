using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulbController : MonoBehaviour
{

    public bool lightOn;
    public float detectionRange;
    public GameObject player;
    public SpriteRenderer sr;
    public Sprite lightBulbOn, lightBulbOff;
    bool closeEnough;
    public GameObject lightSource;
    void Update()
    {
        closeEnough = false;
        if (player)
            if (Vector3.Distance(player.transform.position, transform.position) <= detectionRange)
                closeEnough = true;
        if (Input.GetKeyUp(KeyCode.E) && closeEnough)
            toggleLight();
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = lightOn ? lightBulbOn : lightBulbOff;
    }

    private void toggleLight()
    {
        lightOn = !lightOn;
        sr.sprite = lightOn ? lightBulbOn : lightBulbOff;
        if (lightOn)
        {
            lightSource.SetActive(true);
        }
        else
        {
            lightSource.SetActive(false);
        }
    }
}
