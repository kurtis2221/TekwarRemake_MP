using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {
	
	GameObject playerobj;
	public float targetY;
	public float trigdistance;
	public float movesize;
	public AudioClip open_sound;
	public AudioClip close_sound;
	public AudioClip extra_sound;
	public bool nonstatic = false;
	
	Vector3 startposition;
	Vector3 endposition;
	Vector3 targetposition;
	
	bool iskeydown = false;
	bool isbusy = false;
	bool isadd = false;
	bool isopen = false;
	bool nonstatic_isopen = false;
	
	// Use this for initialization
	void Start () {
		startposition = gameObject.transform.position;
		endposition = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y+targetY,gameObject.transform.position.z);
		movesize /= 10;
	}
	
	// Update is called once per frame
    void FixedUpdate()
	{
		playerobj = GameObject.Find("Player(Clone)");
		if(playerobj != null)
		{
			if(!isopen && !isbusy)
			{
				startposition = gameObject.transform.position;
				endposition = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y+targetY,gameObject.transform.position.z);
			}
			
			if(isbusy)
			{
				if(isadd && targetposition.y >= gameObject.transform.position.y)
					gameObject.transform.position = new Vector3(
						gameObject.transform.position.x,
						gameObject.transform.position.y+movesize,
						gameObject.transform.position.z);
				else if(!isadd && targetposition.y <= gameObject.transform.position.y)
					gameObject.transform.position = new Vector3(
						gameObject.transform.position.x,
						gameObject.transform.position.y-movesize,
						gameObject.transform.position.z);
				else
				{
					gameObject.transform.position = new Vector3(
						gameObject.transform.position.x,
						targetposition.y,
						gameObject.transform.position.z);
					
					if(isopen)
					{
						if(extra_sound != null)
							GetComponent<AudioSource>().PlayOneShot(extra_sound);
						isopen = false;
					}
					else
					{
						if(extra_sound != null)
							GetComponent<AudioSource>().PlayOneShot(extra_sound);
						isopen = true;
					}
					isbusy = false;
				}
			}
			else if(Vector3.Distance(playerobj.transform.position,startposition) <= trigdistance && !nonstatic)
			{
				if(Input.GetButtonDown("Use") && !iskeydown)
				{
					if(Vector3.Equals(gameObject.transform.position,startposition))
					{
						targetposition = endposition;
						GetComponent<AudioSource>().PlayOneShot(open_sound);
						isbusy = true;
						if(targetposition.y >= gameObject.transform.position.y) isadd = true;
						if(targetposition.y <= gameObject.transform.position.y) isadd = false;
					}
					else
					{
						targetposition = startposition;
						GetComponent<AudioSource>().PlayOneShot(close_sound);
						isbusy = true;
						if(targetposition.y >= gameObject.transform.position.y) isadd = true;
						if(targetposition.y <= gameObject.transform.position.y) isadd = false;
					}
					GetComponent<NetworkView>().RPC("OperateDoor",RPCMode.Others);
					iskeydown = true;
				}
				else iskeydown = false;
			}
			else if(Vector3.Distance(gameObject.transform.position,playerobj.transform.position) <= trigdistance && nonstatic)
			{
				if(Input.GetButtonDown("Use") && !iskeydown)
				{	
					if(!nonstatic_isopen)
					{
						targetposition = endposition;
						GetComponent<AudioSource>().PlayOneShot(open_sound);
						nonstatic_isopen = true;
						isbusy = true;
						if(targetposition.y >= gameObject.transform.position.y) isadd = true;
						if(targetposition.y <= gameObject.transform.position.y) isadd = false;
					}
					else
					{
						targetposition = startposition;
						GetComponent<AudioSource>().PlayOneShot(close_sound);
						nonstatic_isopen = false;
						isbusy = true;
						if(targetposition.y >= gameObject.transform.position.y) isadd = true;
						if(targetposition.y <= gameObject.transform.position.y) isadd = false;
					}
					GetComponent<NetworkView>().RPC("OperateDoor2",RPCMode.Others);
					iskeydown = true;
				}
				else iskeydown = false;
			}
		}
	}
	
	[RPC]
	void OperateDoor()
	{
		if(Vector3.Equals(gameObject.transform.position,startposition))
		{
			targetposition = endposition;
			GetComponent<AudioSource>().PlayOneShot(open_sound);
			isbusy = true;
			if(targetposition.y >= gameObject.transform.position.y) isadd = true;
			if(targetposition.y <= gameObject.transform.position.y) isadd = false;
		}
		else
		{
			targetposition = startposition;
			GetComponent<AudioSource>().PlayOneShot(close_sound);
			isbusy = true;
			if(targetposition.y >= gameObject.transform.position.y) isadd = true;
			if(targetposition.y <= gameObject.transform.position.y) isadd = false;
		}
	}
	
	[RPC]
	void OperateDoor2()
	{
		if(!nonstatic_isopen)
		{
			targetposition = endposition;
			GetComponent<AudioSource>().PlayOneShot(open_sound);
			nonstatic_isopen = true;
			isbusy = true;
			if(targetposition.y >= gameObject.transform.position.y) isadd = true;
			if(targetposition.y <= gameObject.transform.position.y) isadd = false;
		}
		else
		{
			targetposition = startposition;
			GetComponent<AudioSource>().PlayOneShot(close_sound);
			nonstatic_isopen = false;
			isbusy = true;
			if(targetposition.y >= gameObject.transform.position.y) isadd = true;
			if(targetposition.y <= gameObject.transform.position.y) isadd = false;
		}	
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(gameObject.transform.position,trigdistance);
	}
}