using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditList : MonoBehaviour {

    public Text creditlist;
    private bool isActiveList;

    private void Start()
    {
        isActiveList = false;
    }

    public void EnableCreditlist(bool dummy)
    {
        if (creditlist != null)
        {
            if (!isActiveList)
            {
                creditlist.enabled = true;
                isActiveList = true;
            }
            else
            {
                creditlist.enabled = false;
                isActiveList = false;
            }
        }
        else
        {
            Debug.Log("NULL CL");
        }
    }
}
