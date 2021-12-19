using UnityEngine;
using System.Collections;

public class NetHit : MonoBehaviour {
	
	GameObject gamecontrol;
	GameObject gui_killinfo;
	GameObject gui_score;
	public int count = 0;
	
	void Start()
	{
		gamecontrol = GameObject.Find("GameControl");
		gui_killinfo = GameObject.Find("HudKilled");
		gui_score = GameObject.Find("HudScore");
		if(GetComponent<NetworkView>().isMine) StartCoroutine(HideKilledBy());
		else enabled = false;
	}
	
	public void SendHit (int hp, int rnd_min, int rnd_max, NetworkViewID id, NetworkPlayer kid, bool isnpc) {
		if(isnpc && Network.isServer)
			SendNetHitServ(hp, rnd_min, rnd_max);
		else
		GetComponent<NetworkView>().RPC("SendNetHit", id.owner, hp, rnd_min, rnd_max,
			kid, gamecontrol.GetComponent<GameMenu>().player_name,isnpc);
	}
	
	[RPC]
	void SendNetHit(int hp, int rnd_min, int rnd_max, NetworkPlayer kid, string kname, bool isnpc)
	{
		if(GetComponent<NetworkView>().isMine)
		{
			if(gameObject.GetComponent<MainScript>().hp - hp <= 0)
			{
				if(isnpc)
					GetComponent<NetworkView>().RPC("SendKilledByNPC",RPCMode.All,gamecontrol.GetComponent<GameMenu>().player_name);
				else
				{
					gamecontrol.GetComponent<NetScore>().ServAddScore(kname);
					GetComponent<NetworkView>().RPC("SendKilledBy",RPCMode.All,gamecontrol.GetComponent<GameMenu>().player_name,kname);
				}
			}
			
			gameObject.GetComponent<MainScript>().hp -= hp;
					
			if(gameObject.GetComponent<MainScript>().cos < 0)
				gameObject.GetComponent<MainScript>().cos = 0;
			else if(gameObject.GetComponent<MainScript>().cos != 0)
			{
				gameObject.GetComponent<MainScript>().cos -= Random.Range(rnd_min,rnd_max);
				if(gameObject.GetComponent<MainScript>().cos < 0)
				gameObject.GetComponent<MainScript>().cos = 0;
			}
		}
		else GetComponent<NetworkView>().RPC("SendNetHit", GetComponent<NetworkView>().owner, hp, rnd_min, rnd_max, kid, kname, isnpc);
	}
	
	void SendNetHitServ(int hp, int rnd_min, int rnd_max)
	{
		if(gameObject.GetComponent<MainScript>().hp - hp <= 0)
			GetComponent<NetworkView>().RPC("RecieveKilledByNPC",RPCMode.All,gamecontrol.GetComponent<GameMenu>().player_name);
		
		gameObject.GetComponent<MainScript>().hp -= hp;
				
		if(gameObject.GetComponent<MainScript>().cos < 0)
			gameObject.GetComponent<MainScript>().cos = 0;
		else if(gameObject.GetComponent<MainScript>().cos != 0)
		{
			gameObject.GetComponent<MainScript>().cos -= Random.Range(rnd_min,rnd_max);
			if(gameObject.GetComponent<MainScript>().cos < 0)
			gameObject.GetComponent<MainScript>().cos = 0;
		}
	}
	
	[RPC]
	void SendKilledBy(string pname, string kname)
	{
		if(kname == gamecontrol.GetComponent<GameMenu>().player_name)
			gui_score.GetComponent<ScoreScript>().score += 1000;
		
		gui_killinfo.GetComponent<GUIText>().text = kname + " killed " + pname;
		gui_killinfo.GetComponent<GUIText>().enabled = true;
		gamecontrol.GetComponent<GameMenu>().pobj.GetComponent<NetHit>().count = 3;
	}
	
	[RPC]
	void RecieveKilledByNPC(string pname)
	{
		gui_killinfo.GetComponent<GUIText>().text = pname + " was killed by an NPC";
		gui_killinfo.GetComponent<GUIText>().enabled = true;
		gamecontrol.GetComponent<GameMenu>().pobj.GetComponent<NetHit>().count = 3;
	}
	
	IEnumerator HideKilledBy()
	{
		while(true)
		{
		yield return new WaitForSeconds(1.0f);
		if(count > 0) count -= 1;
		if(count == 0 && gui_killinfo.GetComponent<GUIText>().enabled) gui_killinfo.GetComponent<GUIText>().enabled = false;
		}
	}
}
