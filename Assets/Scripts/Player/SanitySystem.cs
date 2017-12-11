using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanitySystem : MonoBehaviour
{
    public Slider sanityBar;
    public float sanity;
    private int inSafeZone;
    private bool seen;

    // Use this for initialization
    void Start()
    {
        inSafeZone = 0;
        sanity = 100.0f;
        seen = false;
        sanityBar.maxValue = 100.0f;
        sanityBar.minValue = 0.0f;
        sanityBar.value = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsSafe())
        {
            if(seen)    // If seen we increase the rate of sanity loss
                sanity -= 12.5f * Time.deltaTime;
            else
                sanity -= 7.5f * Time.deltaTime;

            if(sanity < 0.0f)
                sanity = 0.0f;
        }
        else if (IsSafe())
        {
            if(seen)    // If seen we reduce the rate of sanity gain
                sanity += 7.5f * Time.deltaTime;
            else
                sanity += 12.5f * Time.deltaTime;

            if(sanity > 100.0f)
                sanity = 100.0f;
        }
        sanityBar.value = sanity;
    }

    public void isSeen(bool state)
    {
        seen = state;
    }

    private bool IsSafe()
    {
        return inSafeZone > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Light" && inSafeZone > 0)
        {
            Light lightcomp = collision.gameObject.GetComponent<Light>();
            if (lightcomp.enabled)
                inSafeZone--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Light") {
            Light lightcomp = collision.gameObject.GetComponent<Light>();
            if(lightcomp.enabled)
                inSafeZone++;
        }
    }
}
