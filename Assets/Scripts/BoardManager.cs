using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
    public int level;   // Begins at 0, then next floor = 1, and next floor = 2 etc...
    
    public GameObject player;
    public Level[] levels;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    public void SetupScene(int level)
    {
        boardHolder = new GameObject("Board").transform;
        levels[level].SetupScene(boardHolder);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
	void Update () {
		
	}
}
