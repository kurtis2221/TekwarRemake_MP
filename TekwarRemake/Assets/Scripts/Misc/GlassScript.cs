using UnityEngine;
using System.Collections;

public class GlassScript : MonoBehaviour {
	
	public Mesh broken_mesh;
	public AudioClip glass_snd;
	
	void Start()
	{
		
	}
	
	public void Break()
	{
		GetComponent<NetworkView>().RPC("BreakMP",RPCMode.All);
	}
	
	[RPC]
	void BreakMP()
	{
		GetComponent<Collider>().enabled = false;
		GetComponent<AudioSource>().PlayOneShot(glass_snd);
		GetComponent<MeshFilter>().mesh = broken_mesh;
	}
}
