using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public float speed;
    public float rotationalSpeed;
    private SanitySystem sanitySystem;

    AudioSource audioSource;

    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        sanitySystem = this.GetComponentInChildren<SanitySystem>();
    }

    public void Update()
    {
        Move2();
        if (sanitySystem.sanity < 10.0f)
            speed = 1;
        else
            speed = 2;
    }

    private void Move1()
    {
        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Move object
        float x = transform.position.x + (moveX * speed * Time.deltaTime);
        float y = transform.position.y + (moveY * speed * Time.deltaTime);

        // TODO: Rotate object

        // Animate object
        if (moveX != 0.0f || moveY != 0.0f)
            animator.SetTrigger("playerWalkStart");
        else
            animator.SetTrigger("playerWalkEnd");

        transform.position = new Vector2(x, y);
    }

    private void Move2()
    {
        // Rotate
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            float angle = 0.0f;
            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                angle = rotationalSpeed;
            else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                angle = -1 * rotationalSpeed;
            transform.Rotate(0.0f, 0.0f, angle);
        }

        // Move
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            // Start player-walk animation
            animator.SetTrigger("playerWalkStart");

            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                transform.position += transform.right * speed * Time.deltaTime;
            else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
                transform.position += (-1 * transform.right) * (speed / 2) * Time.deltaTime;
            if (audioSource.isPlaying == false)
            {
                audioSource.volume = Random.Range(0.2f, 0.4f);
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.Play();
            }

        }
        else
            // Stop player-walk animation
            animator.SetTrigger("playerWalkEnd");
    }
}
