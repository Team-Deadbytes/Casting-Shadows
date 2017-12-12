using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLightChild : MonoBehaviour
{
	public float FlickerThreshold;

	[SerializeField]
	private CeilingLight parent;

	public void Start()
	{
		parent = transform.parent.GetComponent<CeilingLight>();
	}

	public bool ShouldFlicker()
	{
		return parent.LightBulb.Lifespan <= FlickerThreshold;
	}

	public void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player" && !other.isTrigger)
		{
			if (parent.LightIsOn)
			{
				parent.LightBulb.Lifespan -= Time.deltaTime;
				if (parent.LightBulb.Lifespan <= 0)
					parent.BreakLightBulb();
				else if (!parent.IsFlickering && ShouldFlicker())
					parent.Flicker();
			}
		}
	}
}
