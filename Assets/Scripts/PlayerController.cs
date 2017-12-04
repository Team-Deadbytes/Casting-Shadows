using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
        public float speed;

        private Animator animator;

        public void Start()
        {
                animator = GetComponent<Animator>();
        }

        public void Update()
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
}
