using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerController : MonoBehaviour {
    private SpriteRenderer sr;

    [SerializeField]
    public Sprite closedSprite, open1Sprite, open2Sprite;

    [SerializeField]
    public bool open1, open2;

    // Use this for initialization
    void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
        if(open1)
        {
            sr.sprite = open1Sprite;
        }
        else if(open2)
        {
            sr.sprite = open2Sprite;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (open1)
        {
            sr.sprite = open1Sprite;
        }
        else if (open2)
        {
            sr.sprite = open2Sprite;
        }
        else
        {
            sr.sprite = closedSprite;
        }
    }
}
