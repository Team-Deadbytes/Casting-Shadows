using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour {
    public BoardManager boardScript;
    
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(0);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
