using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLightChild : MonoBehaviour
{
	public float TotalLightBulbLifetime;

	[SerializeField]
	private float remainingLightBulbLifetime;
	private Light lightComponent;

	public void Start()
	{
		lightComponent = GetComponent<Light>();
		remainingLightBulbLifetime = TotalLightBulbLifetime;
	}

	public void Renew()
	{
		remainingLightBulbLifetime = TotalLightBulbLifetime;
	}

	public void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player" && !other.isTrigger)
		{
			if (lightComponent.enabled)
			{
				remainingLightBulbLifetime -= Time.deltaTime;
				if (remainingLightBulbLifetime <= 0)
					transform.parent.GetComponent<CeilingLight>().BreakLightBulb();
			}
		}
	}
}
