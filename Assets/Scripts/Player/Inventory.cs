using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public Stack<LightBulb> LightBulbs;
	public int InitialLightBulbCount;
	public float LightBulbLifespan;

	public void Start()
	{
		LightBulbs = new Stack<LightBulb>();
		for (int i = 0; i < InitialLightBulbCount; i++)
			LightBulbs.Push(new LightBulb(LightBulbLifespan));
	}

	public void AddLightBulb(LightBulb lightBulb)
	{
		LightBulbs.Push(lightBulb);
	}

	public LightBulb RemoveLightBulb()
	{
		if (LightBulbs.Count > 0)
			return LightBulbs.Pop();
		return null;
	}
}

public class LightBulb
{	
	public float Lifespan;

	public LightBulb(float lifespan) { Lifespan = lifespan; }
}
