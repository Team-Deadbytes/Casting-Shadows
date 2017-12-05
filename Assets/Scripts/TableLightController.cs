using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableLightController : MonoBehaviour
{
    private SpriteRenderer renderer;
    public Sprite[] sprites;

    [SerializeField]
    bool state;

    // Use this for initialization
    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        if (state)
            renderer.sprite = sprites[state ? 1 : 0];
    }

    // Update is called once per frame
    void Update()
    {
    }
}
