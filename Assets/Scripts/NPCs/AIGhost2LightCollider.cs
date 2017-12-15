using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGhost2LightCollider : MonoBehaviour {
    AIGhost2 parObject;

    // Use this for initialization
    void Start () {
        parObject = GetComponentInParent<AIGhost2>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "MonsterCollider" && other.tag == "Untagged")
            parObject.onTriggerEnterLight(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "MonsterCollider" && other.tag == "Untagged")
            parObject.onTriggerExitLight(other);
    }
}
