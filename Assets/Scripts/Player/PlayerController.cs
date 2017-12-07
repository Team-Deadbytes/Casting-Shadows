﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public float speed;

    public float[] speedBezierCurvePoints;
    public float speedBezierCurveStart, speedBezierCurveStop;

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

    private float BezierCurve(float sanity)
    {
        float i = Mathf.Abs((sanity - speedBezierCurveStart) / (speedBezierCurveStop - speedBezierCurveStart));
        float res = 0;
        for(int point = 0; point < speedBezierCurvePoints.Length; point++)
        {
            float con = 3.0f;
            if(point == 0  || point == speedBezierCurvePoints.Length - 1)
                con = 1.0f;
            res += con
                * Mathf.Pow(1 - i, speedBezierCurvePoints.Length - point - 1)
                * Mathf.Pow(i, point)
                * speedBezierCurvePoints[point];
        }
        return res;
    }

    public void Update()
    {
        Move2();

        if (sanitySystem.sanity < speedBezierCurveStart && sanitySystem.sanity > speedBezierCurveStop)
            speed = BezierCurve(sanitySystem.sanity);
        else if (sanitySystem.sanity >= speedBezierCurveStart)
            speed = speedBezierCurvePoints[0];
        else
            speed = speedBezierCurvePoints[speedBezierCurvePoints.Length - 1];
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
                angle = rotationalSpeed * Time.deltaTime;
            else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                angle = -1 * rotationalSpeed * Time.deltaTime;
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