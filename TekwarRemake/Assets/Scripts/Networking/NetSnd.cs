using UnityEngine;
using System.Collections;

public class NetSnd : MonoBehaviour {
	
	public AudioClip[] sound;
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void RecSnd(NetworkViewID netid, int snd)
	{
		GetComponent<NetworkView>().RPC("SendGunShot",RPCMode.Others,netid,snd);	
	}
	
	[RPC]
	void SendGunShot(NetworkViewID netid, int sndid)
	{
		NetworkView enemyID = NetworkView.Find(netid);
   		GameObject enemy = enemyID.observed.gameObject;
		enemy.GetComponent<AudioSource>().PlayOneShot(sound[sndid]);
	}
}
