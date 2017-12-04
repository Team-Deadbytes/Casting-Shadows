using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;

	public void Update()
	{
        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // TODO: Rotate object

        // Move object
        float x = transform.position.x + (moveX * speed * Time.deltaTime);
        float y = transform.position.y + (moveY * speed * Time.deltaTime);

        transform.position = new Vector2(x, y);
	}
}
