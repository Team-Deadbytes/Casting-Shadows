using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLightChild : MonoBehaviour
{
	public float TotalLightBulbLifetime;
	public float FlickerThreshold;

	[SerializeField]
	private float remainingLightBulbLifetime;
	private CeilingLight parent;

	public void Start()
	{
		parent = transform.parent.GetComponent<CeilingLight>();
	}

	public void Renew()
	{
		remainingLightBulbLifetime = TotalLightBulbLifetime;
	}

	public bool ShouldFlicker()
	{
		return remainingLightBulbLifetime <= FlickerThreshold;
	}

	public void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player" && !other.isTrigger)
		{
			if (parent.LightIsOn)
			{
				remainingLightBulbLifetime -= Time.deltaTime;
				if (remainingLightBulbLifetime <= 0)
					parent.BreakLightBulb();
				else if (!parent.IsFlickering && ShouldFlicker())
					parent.Flicker();
			}
		}
	}
}
