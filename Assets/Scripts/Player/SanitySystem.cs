using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanitySystem : MonoBehaviour
{
    public float sanity;
    private int inSafeZone;
    private bool seen;

    // Use this for initialization
    void Start()
    {
        inSafeZone = 0;
        sanity = 100.0f;
        seen = false;
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
        if (collision.tag == "Light" && collision.isActiveAndEnabled && inSafeZone > 0)
            inSafeZone--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Light" && collision.isActiveAndEnabled)
            inSafeZone++;
    }
}
