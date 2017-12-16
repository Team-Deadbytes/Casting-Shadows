using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour {

    private Animator anim;
    
    void Start() {
        anim = GetComponent<Animator>();
        anim.SetBool("playtrigger", false);
    }

    public void StartCreditList()
    {
        anim.SetBool("playtrigger", true);
    }

    public void StopAnimation()
    {
        anim.SetBool("playtrigger", false);
    }
}
