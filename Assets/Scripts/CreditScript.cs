using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour {

    private Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
        anim.StopPlayback();
        anim.SetBool("playtrigger", false);
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
