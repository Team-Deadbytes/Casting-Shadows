using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanitySystem : MonoBehaviour
{

    public float sanity;
    private bool inSafeZone;

    // Use this for initialization
    void Start()
    {
        sanity = 20.0f;
        inSafeZone = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inSafeZone && sanity > 0.0f)
        {
            sanity -= 1.5f * Time.deltaTime;
        }
        else if (inSafeZone && sanity < 20.0f)
        {
            sanity += 2.5f * Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Light")
        {
            inSafeZone = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Light")
        {
            inSafeZone = true;
        }
    }
}
