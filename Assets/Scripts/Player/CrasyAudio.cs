using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrasyAudio : MonoBehaviour
{

    AudioSource audioSource;
    SanitySystem sanity;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sanity = GameObject.Find("Units/Player/Player top light").GetComponent<SanitySystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sanity.sanity <= 40)
            audioSource.volume = 1;
        else
            audioSource.volume = 0;
    }
}
