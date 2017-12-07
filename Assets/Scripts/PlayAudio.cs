using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudio : MonoBehaviour {
    private AudioSource audioSource;

    private void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.isTrigger == false)
        {
            audioSource = GetComponent<AudioSource>();
            if(!audioSource.isPlaying)
                audioSource.Play();
        }
}
}
