using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGhost2 : MonoBehaviour
{
    [SerializeField]
    private float patrolSpeed, chaseSpeed;
    [SerializeField]
    private GameObject aStarGraph;

    private GameObject player;
    private GameObject self;
    private AIPath pathing;
    private AstarPath graph;
    private GameMgr GameMgr;
    private bool isPlayerClose;
    GameObject target;

    bool runningFromLight;
    Vector3 lightAwayDirection;
    float runningAwayTimeout;

    // These variables are to monitor whether the player is stuck
    private float distanceTravelled, timeSpentTravelling;
    private Vector3 lastPos;

    private Vector3 newTarget()
    {
        float width, height;
        Vector3 center = graph.data.gridGraph.center;
        width = graph.data.gridGraph.width * graph.data.gridGraph.nodeSize;
        height = graph.data.gridGraph.depth * graph.data.gridGraph.nodeSize;

        float x, y;
        x = center.x - (width / 2.0f) + Random.Range(0, width);
        y = center.y - (height / 2.0f) + Random.Range(0, height);

        return new Vector3(x, y, center.z);
    }

    public void pathingDestinationReached()
    {
        if(pathing.target == player.transform)
        {
            player.GetComponent<PlayerController>().Die();
            GameMgr.Die();
        } else
        {
            StartCoroutine(Wait());
            pathing.target.position = newTarget();
        }
    }

    //distanceTravelled, timeSpentTravelling
    private void checkDistanceTravelled()
    {
        distanceTravelled += (lastPos - self.transform.position).magnitude;
        lastPos.x = self.transform.position.x;
        lastPos.y = self.transform.position.y;
        lastPos.z = self.transform.position.z;
        timeSpentTravelling += Time.deltaTime;
        if(timeSpentTravelling > 6.0f)
        {
            if(distanceTravelled < 1.0f)
                pathing.target.position = newTarget();

            distanceTravelled = 0.0f;
            timeSpentTravelling = 0.0f;
        }
    }

    // Use this for initialization
    void Start()
    {
        GameMgr = GameObject.Find("_GameManager").GetComponent<GameMgr>();
        if(GameMgr == null)
            Debug.LogError("AIGhost2 Script is unable to find the Game Manager");

        player = GameObject.Find("Player");
        if (player == null)
            Debug.LogError("AIGhost2 Script is unable to find the player GameObject");
        
        self = gameObject;
        if (self == null)
            Debug.LogError("AIGhost2 Script is unable to find the self GameObject");

        pathing = GetComponent<AIPath>();
        if (pathing == null)
            Debug.LogError("AIGhost2 Script is unable to find the AIPath GameObject component");

        graph = aStarGraph.GetComponent<AstarPath>();
        if (graph == null)
            Debug.LogError("AIGhost2 Script is unable to find the AstarPath GameObject compopnent");

        pathing.speed = patrolSpeed;
        isPlayerClose = false;
        pathing.aiInstance = this;

        target = new GameObject(self.name + " Target");
        target.transform.position = newTarget();
        pathing.target = target.transform;

        lastPos = new Vector3(self.transform.position.x, self.transform.position.y, self.transform.position.z);

        distanceTravelled = 0.0f;
        timeSpentTravelling = 0.0f;
        runningFromLight = false;
        runningAwayTimeout = float.PositiveInfinity;
    }

    // Update is called once per frame
    void Update()
    {
        checkDistanceTravelled();

        if (runningFromLight)
        {
            self.transform.position += lightAwayDirection * Time.deltaTime;
            if (runningAwayTimeout != float.PositiveInfinity)
            {
                runningAwayTimeout -= Time.deltaTime;
                if(runningAwayTimeout < 0.0f)
                {
                    runningFromLight = false;
                    runningAwayTimeout = float.PositiveInfinity;
                    pathing.target = target.transform;
                    target.transform.position = newTarget();
                    pathing.speed = patrolSpeed;
                }
            }
        }
    }

    public void onTriggerEnterPlayer(Collider2D other)
    {
        //Debug.Log("Player enter: " + other.name + " " + other.tag);
        if (runningFromLight)
            return;
        pathing.target = player.transform;
        pathing.speed = chaseSpeed;
    }

    public void onTriggerExitPlayer(Collider2D other)
    {
        //Debug.Log("Player exit: " + other.name + " " + other.tag);
        if (runningFromLight)
            return;
        pathing.target = target.transform;
        pathing.speed = patrolSpeed;
    }

    public void onTriggerEnterLight(Collider2D other)
    {
        //Debug.Log("Light Enter: " + other.name + " " + other.tag);
        runningFromLight = true;
        CeilingLight lightsystem = other.GetComponentInParent<CeilingLight>();
        lightsystem.MonsterProwing(true);
        lightAwayDirection = (gameObject.transform.position - other.transform.position).normalized;
        pathing.speed = 0;
    }

    public void onTriggerExitLight(Collider2D other)
    {
        //Debug.Log("Light Exit: " + other.name + " " + other.tag);
        runningAwayTimeout = 1.0f;
        CeilingLight lightsystem = other.GetComponentInParent<CeilingLight>();
        lightsystem.MonsterProwing(false);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(Random.Range(1, 4));
    }
}