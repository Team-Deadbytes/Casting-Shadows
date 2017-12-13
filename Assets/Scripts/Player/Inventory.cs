using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public Stack<LightBulb> LightBulbs;
	public int InitialLightBulbCount;
	public float LightBulbLifespan;

    private GameObject LightbulbCounterUI;
    private Text LightbulbCounterText;


    public void Start()
	{
		LightBulbs = new Stack<LightBulb>();
		for (int i = 0; i < InitialLightBulbCount; i++)
			LightBulbs.Push(new LightBulb(LightBulbLifespan));
        LightbulbCounterUI = GameObject.Find("/Canvas/LightbulbCounter");
        if (LightbulbCounterUI != null)
        { 
            LightbulbCounterText = LightbulbCounterUI.GetComponent<Text>();
            LightbulbCounterText.text = InitialLightBulbCount.ToString();
        }
    }

	public void AddLightBulb(LightBulb lightBulb)
	{
		LightBulbs.Push(lightBulb);
        LightbulbCounterText.text = LightBulbs.Count.ToString();
    }

	public LightBulb RemoveLightBulb()
	{
		if (LightBulbs.Count > 0)
        {
            LightbulbCounterText.text = (LightBulbs.Count - 1).ToString();
            return LightBulbs.Pop();
        }
        return null;
	}
}

public class LightBulb
{	
	public float Lifespan;

	public LightBulb(float lifespan) { Lifespan = lifespan; }
}
