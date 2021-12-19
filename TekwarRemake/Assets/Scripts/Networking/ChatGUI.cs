using UnityEngine;
using System.Collections;

public class ChatGUI : MonoBehaviour {

	GUIText chat;
	int count = 0;
	
	void Start()
	{
		chat = GameObject.Find("HudChat").GetComponent<GUIText>();
		StartCoroutine(HideChat());
	}
	
	public void MessageTrigger(string msg)
	{
		GetComponent<NetworkView>().RPC("ShowChat",RPCMode.All,msg);
	}
	
	[RPC]
	void ShowChat(string msg)
	{
		chat.enabled = true;
		chat.text = msg;
		count = 5;
	}
	
	IEnumerator HideChat()
	{
		while(true)
		{
			yield return new WaitForSeconds(1.0f);
			if(count > 0) count-=1;
			else if(count == 0 && chat.enabled) chat.enabled = false;
		}
	}
}
