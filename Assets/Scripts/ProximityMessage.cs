using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityMessage : MonoBehaviour
{
	public string Message;
	private bool show;

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && !other.isTrigger)
			show = true;
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player" && !other.isTrigger)
			show = false;
	}

	private void OnGUI()
	{
		if (show)
		{
			GUIStyle labelStyle = GUI.skin.GetStyle("Label");
			labelStyle.alignment = TextAnchor.UpperCenter;

			float labelWidth = 300f;
			float labelHeight = 50f;
			float labelX = (Screen.width - labelWidth) / 2;
			float labelY = Screen.height - 100;
			Rect labelRect = new Rect(labelX, labelY, labelWidth, labelHeight);

			GUI.Label(labelRect, Message, labelStyle);
		}
	}
}
