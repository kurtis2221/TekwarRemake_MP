using UnityEngine;
using System.Collections;

public class Validation : MonoBehaviour {
	
	public int version = 0;
	public int level = 0;
	GameObject[] activeplayers;
	
	void Start()
	{
		
	}
	
	public void SetPlayer () {
		if(GameObject.Find("park")) level = 1;
		else if(GameObject.Find("mid2")) level = 2;
		version = 171;
		
		if(Network.isClient)
			GetComponent<NetworkView>().RPC("SendValidInfo",RPCMode.Server,version,level,GetComponent<NetworkView>().viewID);
		else if(Network.isServer)
			StartCoroutine(WaitValid());	
	}
	
	IEnumerator WaitValid()
	{
		yield return new WaitForSeconds(10.0f);
		activeplayers = GameObject.FindGameObjectsWithTag("NetworkPlayer");
		for(int i = 0; i < activeplayers.Length; i++)
		{
			if(activeplayers[i].GetComponent<Validation>().version != version ||
				activeplayers[i].GetComponent<Validation>().level != level)
				Network.CloseConnection(activeplayers[i].GetComponent<NetworkView>().viewID.owner,true);
		}
		StartCoroutine(WaitValid());
	}
	
	[RPC]
	void SendValidInfo(int ver, int lvl, NetworkViewID netid)
	{
		if(Network.isServer)
		{
			NetworkView enemyID = NetworkView.Find(netid);
   			GameObject enemy = enemyID.observed.gameObject;
			enemy.GetComponent<Validation>().level = lvl;
			enemy.GetComponent<Validation>().version = ver;
		}
	}
}
