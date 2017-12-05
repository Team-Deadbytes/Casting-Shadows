using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class BloodStainController : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField]
    public Sprite[] sprites;
    [SerializeField]
    public bool keepDefault;

    void Awaken()
    {
        keepDefault = false;
    }

    // Use this for initialization
    void Start()
    {
        if (!keepDefault)
        {
            sr = gameObject.GetComponent<SpriteRenderer>();
            sr.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
