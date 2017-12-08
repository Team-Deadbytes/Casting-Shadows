using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceToMonsters : MonoBehaviour
{
    public GameObject[] monsters;
    public GameObject player; // two objects that generate noise if they are close together
    private GameObject minMonster;
    private float minDist, distBetween;

    // Use this for initialization
    void Start()
    {
        if (monsters.Length == 0 || !player) // if objects are not set disable the effect
            enabled = false;
    }

    // Update is called once per frame
    // returns a value between 0 and 1, based on minimum distance between player and monsters used  to multiply with effects
    public float getMinDistMultiplyer(float maxDistance)
    {
        minDist = float.MaxValue; // set min dist to float max value
        if (monsters.Length != 0 || player) // if there are monsters and player
        {
            foreach (GameObject monster in monsters)
            {
                // if there is a monster with shorter distance to player update minDist and minMonster
                float tmpDist = Vector3.Distance(monster.transform.position, player.transform.position);
                if (tmpDist < minDist)
                {
                    minMonster = monster;
                    minDist = tmpDist;
                }
            }
            distBetween = Mathf.Clamp(Vector3.Distance(minMonster.transform.position, player.transform.position), 0.0f, maxDistance) / maxDistance; // get the distance between x and y in 0 to 1 formant
            distBetween = Mathf.Abs(distBetween - 1); // change from 0 to 1, to 1 to 0
        }
        return distBetween;
    }
}
