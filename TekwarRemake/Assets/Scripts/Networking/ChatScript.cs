using UnityEngine;
using System.Collections;

public class ChatScript : MonoBehaviour {
	
	string message = "";
	
	void OnGUI()
	{
		if (Event.current.Equals(Event.KeyboardEvent("return")))
		{
			if(message.Length > 0)
				GetComponent<ChatGUI>().MessageTrigger(GetComponent<GameMenu>().player_name + ": " + message);
			message = "";
			enabled = false;
		}
		else if (Event.current.Equals(Event.KeyboardEvent("escape")))
		{
			message = "";
			enabled = false;
		}
		GUI.Label(new Rect(92,128,24,24),"Say:");
		GUI.SetNextControlName("ChatBox");
		message = GUI.TextField(new Rect(128,128,256,24),message);
		GUI.FocusControl("ChatBox");
	}
}
