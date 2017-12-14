using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour {

    private GameMgr GameMgr;

    // Use this for initialization
    void Start () {
        GameMgr = GameObject.Find("_GameManager").GetComponent<GameMgr>();
	}
	
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.isTrigger == false)
        {
            GameMgr.Win();
        }
    }
}
