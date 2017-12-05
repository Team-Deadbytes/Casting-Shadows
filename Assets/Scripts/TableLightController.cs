using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableLightController : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite[] sprites;

    [SerializeField]
    bool state;

    // Use this for initialization
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        if (state)
            sr.sprite = sprites[state ? 1 : 0];
    }

    // Update is called once per frame
    void Update()
    {
    }
}
