using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGhost2 : MonoBehaviour {
    enum Style { AGGRESIVE, PROWLER, SABOTEUR };
    enum Pathing { PATROLLER, WONDERER };

    [SerializeField]
    Pathing pathingPreference;
    [SerializeField]
    Style strategyPreference;

    //private GameObject player;


	// Use this for initialization
	void Start () {
        //player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
