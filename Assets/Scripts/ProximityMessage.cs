using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityMessage : MonoBehaviour
{
	public string Message;
    public bool TypeEffect;
    private bool show;
    public int height;
    private string tmpMessage;
    public int FontSize;

    private void Start()
    {
        if (height == 0)
            height = 100;
        if (FontSize == 0)
            FontSize = 20;
    }

    public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && !other.isTrigger)
        { 
			show = true;
            if (TypeEffect)
            {
                tmpMessage = Message;
                Message = "";
                StartCoroutine(TypeF());
            }
        }
    }

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player" && !other.isTrigger)
        { 
			show = false;
            StopAllCoroutines();
            if(TypeEffect)
                Message = tmpMessage;
        }
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
			float labelY = Screen.height - height;
			Rect labelRect = new Rect(labelX, labelY, labelWidth, labelHeight);

			GUI.Label(labelRect, Message, labelStyle);
		}
	}

    IEnumerator TypeF()
    {
        if (show)
        {
            foreach (char letter in tmpMessage.ToCharArray())
            {
                Message += letter;
                //if (typeSound1 && typeSound2)
                //  SoundManager.instance.RandomizeSfx(typeSound1, typeSound2);
                yield return 0;
                yield return new WaitForSeconds(Random.Range(0.05f,0.15f));
            }
        }
    }
 }
