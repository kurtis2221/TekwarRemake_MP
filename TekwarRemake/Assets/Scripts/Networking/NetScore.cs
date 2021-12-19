using UnityEngine;
using System.Collections;

public class NetScore : MonoBehaviour {
	
	const int MAX_PLAYERS = 16;
	string[] names = new string[MAX_PLAYERS];
	int[] scores = new int[MAX_PLAYERS];
	string list;
	
	public void StartScanning()
	{
		if(Network.isServer)
		{
			names[0] = GetComponent<GameMenu>().player_name;
			scores[0] = 0;
			GameObject.Find("HudPlayers").GetComponent<GUIText>().text =
				names[0] + " - " + scores[0];
		}
	}
	
	[RPC]
	void AddPlayer(string pname, NetworkViewID id)
	{
		for(int i = 0; i < MAX_PLAYERS; i++)
		{
			if(names[i] == pname)
			{
				Network.CloseConnection(id.owner,true);
				break;
			}
			if(names[i] == null)
			{
				names[i] = pname;
				break;
			}
		}
		ScoreUpdate();
	}
	
	[RPC]
	void RemPlayer(string pname)
	{
		for(int i = 0; i < MAX_PLAYERS; i++)
		{
			if(names[i] == pname)
			{
				names[i] = null;
				scores[i] = 0;
				break;
			}
		}
		ScoreUpdate();
	}
	
	[RPC]
	void AddScore(string pname)
	{
		for(int i = 0; i < MAX_PLAYERS; i++)
		{
			if(names[i] == pname)
			{
				scores[i] += 1000;
				break;
			}
		}
		ScoreUpdate();
	}
	
	void ScoreUpdate()
	{
		list = null;
		for(int i = 0; i < MAX_PLAYERS; i++)
		{
			if(names[i] == null) continue;
			list += names[i] + " - " + scores[i] + "\n";
		}
		GameObject.Find("HudPlayers").GetComponent<GUIText>().text = list;
		GetComponent<NetworkView>().RPC("SendScore",RPCMode.Others,list);
	}
	
	[RPC]
	void SendScore(string data)
	{
		list = data;
		GameObject.Find("HudPlayers").GetComponent<GUIText>().text = list;
	}
	
	public void ServAddPlayer(string pname, NetworkViewID id)
	{
		GetComponent<NetworkView>().RPC("AddPlayer",RPCMode.Server,pname,id);
	}
	
	public void ServRemPlayer(string pname)
	{
		GetComponent<NetworkView>().RPC("RemPlayer",RPCMode.Server,pname);
	}
	
	public void ServAddScore(string pname)
	{
		if(Network.isClient)
			GetComponent<NetworkView>().RPC("AddScore",RPCMode.Server,pname);
		else
			AddScore(pname);
	}
}
