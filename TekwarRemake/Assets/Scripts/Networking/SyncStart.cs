using UnityEngine;
using System.Collections;

public class SyncStart : MonoBehaviour {
	
	GameObject[] barrels;
	GameObject[] glasses;
	GameObject[] npcs;
	public Mesh glass_broke;
	int count = 0;
	
	void Start()
	{
		barrels = GameObject.FindGameObjectsWithTag("Barrels");
		glasses = GameObject.FindGameObjectsWithTag("Glasses");
		npcs = GameObject.FindGameObjectsWithTag("NPC");
	}
	
	public void SendSyncCall(NetworkViewID player)
	{
		GetComponent<NetworkView>().RPC("GetSyncCall",RPCMode.Server,player);	
	}
	
	[RPC]
	void GetSyncCall(NetworkViewID player)
	{
		if(barrels.Length > 0)
		{
			for(int i = 0; i < barrels.Length; i++)
			{
				GetComponent<NetworkView>().RPC("ServerSyncCall",player.owner,barrels[i] == null);
			}
		}
		GetComponent<NetworkView>().RPC("ServerSyncCallNull",player.owner);
		if(glasses.Length > 0)
		{
			for(int i = 0; i < glasses.Length; i++)
			{
				GetComponent<NetworkView>().RPC("ServerSyncCall2",player.owner,!glasses[i].GetComponent<Collider>().enabled);
			}
		}
		GetComponent<NetworkView>().RPC("ServerSyncCallNull",player.owner);
		
		if(!GetComponent<GameMenu>().npcenabled)
			GetComponent<NetworkView>().RPC("ServerSyncNoNPC",player.owner);
		else if(npcs.Length > 0)
		{
			for(int i = 0; i < npcs.Length; i++)
			{
				GetComponent<NetworkView>().RPC("ServerSyncCall3",player.owner,!npcs[i].GetComponent<Collider>().enabled,npcs[i].transform.position);
			}
		}
	}
	
	[RPC]
	void ServerSyncCall(bool barrel)
	{
		if(barrel) Destroy(barrels[count]);
		count += 1;
	}
	
	[RPC]
	void ServerSyncCall2(bool glass)
	{
		if(glass)
		{
		glasses[count].GetComponent<MeshFilter>().mesh = glass_broke;
		glasses[count].GetComponent<Collider>().enabled = false;
		}
		count += 1;
	}
	
	[RPC]
	void ServerSyncCall3(bool npc, Vector3 pos)
	{
		if(npc)
		{
			if(npcs[count].name == "civilian")
				npcs[count].GetComponent<civil_nav>().DeathClient();
			else
				npcs[count].GetComponent<cop_nav>().DeathClient();
			npcs[count].transform.position = pos;
		}
		count += 1;
	}
	
	[RPC]
	void ServerSyncNoNPC()
	{
		for(int i = 0; i < npcs.Length; i++)
		{
			Network.RemoveRPCs(npcs[i].GetComponent<NetworkView>().viewID);
			Network.Destroy(npcs[i]);
			Destroy(npcs[i]);
		}
	}
	
	[RPC]
	void ServerSyncCallNull()
	{
		count = 0;
	}
}
