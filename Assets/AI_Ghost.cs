using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Ghost : MonoBehaviour {

    public Transform[] target;
    public float speed;
    private int currTarget;
    // Use this for initialization
    void Start () {
        currTarget = 0;
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target[currTarget].position, step);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (currTarget < (target.Length - 1))
                currTarget = currTarget += 1;
            else
            {
                currTarget = 0;
            }
        }
        else if (currTarget < target.Length)
        {
            currTarget += 1;
        }
        else
        {
            currTarget = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (currTarget < target.Length)
            {
                currTarget = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }
    }
}
