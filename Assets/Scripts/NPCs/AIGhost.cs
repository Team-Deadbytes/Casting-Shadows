using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGhost : MonoBehaviour
{
    public Transform[] target;
    public float patrolSpeed;
    public float chaseSpeed;
    public int currTarget, lastTarget;
    private Collider2D prevCollider;
    private GameObject playerObject;
    private GameObject doorObject;
    public Canvas deathCanvas;

    [SerializeField]
    public float realspeed;
    [SerializeField]
    public float speedCheck;
    Vector3 lastPos;
    int trailback;
    bool waiting;

    private SanitySystem sanitySystem;
    private DoorController doorController;

    // Use this for initialization
    void Start()
    {
        trailback = 0;
        lastPos = this.gameObject.transform.position;
        speedCheck = 0.0f;
        currTarget = 1;
        lastTarget = 1;
        playerObject = GameObject.Find("Player");
        if(playerObject != null)
            sanitySystem = playerObject.GetComponentInChildren<SanitySystem>();
        if (Time.timeScale < 0.1f)
            Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float step;
        if (currTarget == 0)
            step = chaseSpeed * Time.deltaTime;
        else
            step = patrolSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target[currTarget].position, step);
        if(currTarget == 0 && playerObject != null)
            sanitySystem.sanity -= 3.0f * Time.deltaTime;
        if(currTarget == 0)
            sanitySystem.isSeen(true);
        else
            sanitySystem.isSeen(false);

        if (!waiting)
        {
            Vector3 diffpos = lastPos - this.gameObject.transform.position;
            realspeed += diffpos.magnitude;
            speedCheck += Time.deltaTime;
            if (speedCheck > 6.0f)
            {
                if (realspeed < 4.5f)
                {
                    trailback = 2;
                    currTarget -= 1;
                    if (currTarget <= 0)
                    {
                        int num = Random.Range(1, target.Length);
                        Transform waypoint = target[num];
                        this.gameObject.transform.position = waypoint.position;
                        currTarget = num + 1;
                        if (currTarget == target.Length)
                            currTarget = 1;
                    }
                }
                speedCheck = 0.0f;
                realspeed = 0.0f;
            }
            lastPos = this.gameObject.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (prevCollider == null || prevCollider.gameObject.name != other.gameObject.name)
        {
            prevCollider = other;
            if (other.gameObject.tag == "Player")
            {
                lastTarget = currTarget;
                currTarget = 0;
            }
            else if (other.gameObject.tag == "Waypoint" && other.gameObject == target[currTarget].gameObject)
            {
                if (trailback > 0)
                {
                    if(trailback == 1)
                    {
                        lastTarget = currTarget;
                        currTarget += 2;
                        if (currTarget == target.Length)
                            currTarget = 1;
                    } else
                    {
                        lastTarget = currTarget;
                        currTarget--;
                        if (currTarget == 0)
                            currTarget = target.Length - 1;
                    }
                    trailback--;
                } else
                    StartCoroutine(Wait());
            }
            else if (other.gameObject.tag == "Light")
            {
                Light otherlit = other.gameObject.GetComponent<Light>();
                if (otherlit.enabled)
                {
                    Debug.Log("Light " + other.gameObject.tag);
                    CeilingLight lightsystem = other.GetComponentInParent<CeilingLight>();
                    lightsystem.MonsterProwing(true);

                    currTarget = 1;
                }
            }
            else if (other.gameObject.tag == "Untagged")
                return;
            else if (other.gameObject.tag != "Objects" && other.gameObject.tag != "Waypoint")
            {
                Debug.Log("Unknown " + other.gameObject.tag);
                currTarget -= 1;
                trailback = 2;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (currTarget < target.Length - 1)
                currTarget = lastTarget;
        }
        else if (collision.gameObject.tag == "Light" && collision.isActiveAndEnabled)
        {
            CeilingLight lightsystem = collision.GetComponentInParent<CeilingLight>();
            lightsystem.MonsterProwing(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().Die();
            deathCanvas.gameObject.SetActive(true);
            Time.timeScale = 0.00001f;
        }
        if (collision.gameObject.name.StartsWith("WoodenDoor"))
        {
            doorObject = collision.gameObject;
            doorController = doorObject.GetComponent<DoorController>();
            if(!doorController.isOpen)
                doorController.toggleDoor();
        }
    }

    IEnumerator Wait()
    {
        //print(Time.time);
        waiting = true;
        yield return new WaitForSecondsRealtime(Random.Range(0, 4));
        waiting = false;
        lastTarget = currTarget;
        if (currTarget < target.Length - 1)
            currTarget += 1;
        else
            currTarget = 1;
        //print(Time.time);
    }
}
