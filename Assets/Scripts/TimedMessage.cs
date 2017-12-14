using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedMessage : MonoBehaviour
{
	public string Message;
	public int FontSize;
	public int VerticalPosition;
	public float Duration;

	private bool show;
	private float showTimer;
	private string tmpMessage;

	private void Update()
	{
		if (show)
		{
			showTimer += Time.deltaTime;
			if (showTimer >= Duration)
				StopAndHide();
		}
	}

	public void Show()
	{
		StopAndHide();
		show = true;
		StartCoroutine(TypeEffect());
	}
	
	private void StopAndHide()
	{
		StopAllCoroutines();
		show = false;
		tmpMessage = string.Empty;
		showTimer = 0;
	}

	private void OnGUI()
	{
		if (show)
		{
			GUIStyle labelStyle = GUI.skin.GetStyle("Label");
			labelStyle.alignment = TextAnchor.UpperCenter;
			labelStyle.fontSize = FontSize;

			float labelWidth = 600f;
			float labelHeight = 80f;
			float labelX = (Screen.width - labelWidth) / 2;
			float labelY = Screen.height - VerticalPosition;
			Rect labelRect = new Rect(labelX, labelY, labelWidth, labelHeight);

			GUI.Label(labelRect, tmpMessage, labelStyle);
		}
	}

	private IEnumerator TypeEffect()
	{
		if (show)
		{
			foreach (char letter in Message)
			{
				tmpMessage += letter;
				yield return new WaitForSeconds(0.05f);
			}
		}
	}
}