using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour {

    private Animator anim;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        anim.StopPlayback();
        anim.SetBool("playtrigger", false);
    }

    // Update is called once per frame
    void Update() {

    }

    public void StartCreditList()
    {
        this.gameObject.SetActive(true);
        anim.SetBool("playtrigger", true);
        anim.Play("Credidt");
    }

    public void StopAnimation()
    {
        anim.SetBool("playtrigger", false);
    }
}
