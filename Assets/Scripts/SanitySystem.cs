using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanitySystem : MonoBehaviour
{
    public float sanity;
    bool selfLight;
    private Stack<bool> inSafeZone;
    private bool seen;

    // Use this for initialization
    void Start()
    {
        inSafeZone = new Stack<bool>();
        selfLight = true;
        sanity = 20.0f;
        seen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsSafe() && sanity > 0.0f)
        {
            if(seen)    // If seen we increase the rate of sanity loss
            {
                sanity -= 2.5f * Time.deltaTime;
            } else
            {
                sanity -= 1.5f * Time.deltaTime;
            }
            if(sanity < 0.0f)
            {
                sanity = 0.0f;
            }
        }
        else if (IsSafe() && sanity < 20.0f)
        {
            if(seen)    // If seen we reduce the rate of sanity gain
            {
                sanity += 1.5f * Time.deltaTime;
            } else
            {
                sanity += 2.5f * Time.deltaTime;
            }
            if(sanity > 20.0f)
            {
                sanity = 20.0f;
            }
        }
    }

    public void isSeen(bool state)
    {
        seen = state;
    }

    private bool IsSafe()
    {
        return inSafeZone.Count > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Light")
        {
            inSafeZone.Pop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(selfLight)
        {
            selfLight = false;
        }
        else if (collision.tag == "Light")
        {
            inSafeZone.Push(true);
        }
    }
}
