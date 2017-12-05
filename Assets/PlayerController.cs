using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    void Update()
    {
        var x = Input.GetAxis("Vertical") * Time.deltaTime * 6.0f;
        var y = Input.GetAxis("Horizontal") * Time.deltaTime * 6.0f;

        transform.Translate(0, x, 0);
        transform.Translate(y, 0, 0);
    }
}
