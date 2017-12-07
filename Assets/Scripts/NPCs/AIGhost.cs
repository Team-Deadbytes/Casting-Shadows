using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGhost : MonoBehaviour
{
    public Transform[] target;
    public float speed;
    public int currTarget;
    private Collider2D prevCollider;
    private GameObject playerObject;
    private GameObject doorObject;
    public Canvas deathCanvas;

    private SanitySystem sanitySystem;
    private DoorController doorController;

    // Use this for initialization
    void Start()
    {
        currTarget = 1;
        playerObject = GameObject.Find("Player");
        if(playerObject != null)
            sanitySystem = playerObject.GetComponentInChildren<SanitySystem>();
        if (Time.timeScale < 0.1f)
            Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target[currTarget].position, step);
        if(currTarget == 0 && playerObject != null)
            sanitySystem.sanity -= 3.0f * Time.deltaTime;
        if(currTarget == 0)
            sanitySystem.isSeen(true);
        else
            sanitySystem.isSeen(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (prevCollider == null || prevCollider.gameObject.name != other.gameObject.name)
        {
            prevCollider = other;
            if (other.gameObject.tag == "Player")
                currTarget = 0;
            else if (other.gameObject.tag == "Waypoint" && other.gameObject == target[currTarget].gameObject)
                StartCoroutine(Wait());
            else if (other.gameObject.tag == "Light" && other.isActiveAndEnabled)
            {
                CeilingLight lightsystem = other.GetComponentInParent<CeilingLight>();
                lightsystem.MonsterProwing(true);
            }
            else if (other.gameObject.tag != "Objects" && other.gameObject.tag != "Waypoint")
                currTarget = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (currTarget < target.Length - 1)
                currTarget = 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            deathCanvas.gameObject.SetActive(true);
            Time.timeScale = 0.00001f;
        }
        if (collision.gameObject.name.StartsWith("WoodenDoor"))
        {
            doorObject = collision.gameObject;
            doorController = doorObject.GetComponent<DoorController>();
            if(!doorController.isOpen)
            {
                doorController.toggleDoor();
            }
        }
    }

        IEnumerator Wait()
    {
        //print(Time.time);
        yield return new WaitForSecondsRealtime(Random.Range(0, 4));
        if (currTarget < target.Length - 1)
            currTarget += 1;
        else
            currTarget = 1;
        //print(Time.time);
    }
}
