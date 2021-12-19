using UnityEngine;
using System.Collections;

public class DoorScript3 : MonoBehaviour {
	
	public float trigdistance = 0.0f;
	public AudioClip doorsnd;
	bool isbusy = false;
	bool iskeydown = false;
	bool isopen = false;
	GameObject playerobj;
	
	void Start () {
	
	}
	
	void FixedUpdate () {
		if(playerobj == null)
			playerobj = GameObject.Find("Player(Clone)");
		else
		{
			if(Vector3.Distance(gameObject.transform.position,playerobj.transform.position) <= trigdistance && !isbusy)
			{
				if(Input.GetButtonDown("Use") && !iskeydown)
				{
					if(isopen)
					{
						gameObject.GetComponent<Animation>().Play("close");
						isopen = false;
						isbusy = true;
					}
					else
					{
						gameObject.GetComponent<Animation>().Play("open");
						isopen = true;
						isbusy = true;
					}
					GetComponent<AudioSource>().PlayOneShot(doorsnd);
					GetComponent<NetworkView>().RPC("OperateDoor",RPCMode.Others);
					iskeydown = true;
				}
				else iskeydown = false;
			}
			else
			{
				if(!gameObject.GetComponent<Animation>().isPlaying) isbusy = false;
			}
		}
	}
	
	[RPC]
	void OperateDoor()
	{
		if(isopen)
		{
			gameObject.GetComponent<Animation>().Play("close");
			isopen = false;
			isbusy = true;
		}
		else
		{
			gameObject.GetComponent<Animation>().Play("open");
			isopen = true;
			isbusy = true;
		}
		GetComponent<AudioSource>().PlayOneShot(doorsnd);
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(gameObject.transform.position,trigdistance);
	}
}