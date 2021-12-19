using UnityEngine;
using System.Collections;

public class MetroScript : MonoBehaviour {
	
	public GameObject door1;
	public GameObject door2;
	public Vector3[] stations;
	public float vehicle_speed;
	public float speed;
	public float waittime;
	public float movedist;
	public AudioClip doorsounds;
	
	Vector3 startposition1;
	Vector3 endposition1;
	Vector3 startposition2;
	Vector3 endposition2;
	Vector3 offset = new Vector3(20,0,0);
	
	//public Vector3[] stations;
	
	int targetstation = 0;
	bool isstanding = false;
	bool isbusy = false;
	bool isopen = false;
	bool isdelay = false;
	bool ischecked = false;
	bool isforward = true;
	bool isplayed = false;
	
	void Start()
	{
		vehicle_speed /= 10;
		speed /= 10;	
	}
	
	// Update is called once per frame
    	void FixedUpdate ()
	{
		if(Network.isServer)
		{
			if(!isstanding)
			{		
				if(targetstation == stations.Length && isforward)
				{
					isforward = false;
					targetstation -= 2;
				}
				else if(targetstation == -1 && !isforward)
				{
					isforward = true;
					targetstation += 2;
				}
				
				if(isforward)
					gameObject.transform.position = Vector3.Lerp(
						gameObject.transform.position,
						stations[targetstation] - offset,
						Time.deltaTime * vehicle_speed);
				else
					gameObject.transform.position = Vector3.Lerp(
						gameObject.transform.position,
						stations[targetstation] + offset,
						Time.deltaTime * vehicle_speed);
				
				GetComponent<NetworkView>().RPC("MoveMetro",RPCMode.Others,gameObject.transform.position);
				
				if(Vector3.Distance(gameObject.transform.position,stations[targetstation]) < 0.2)
				{
					gameObject.transform.position = stations[targetstation];
					if(isforward) targetstation += 1;
					else targetstation -= 1;
					isbusy = true;
					isstanding = true;
				}
			}
			else if(isstanding && isbusy)
			{
				if(!ischecked)
				{
					startposition1 = door1.transform.position;
					startposition2 = door2.transform.position;
					endposition1 = new Vector3(
						door1.transform.position.x+movedist,
						door1.transform.position.y,
						door1.transform.position.z);
					endposition2 = new Vector3(
						door2.transform.position.x-movedist,
						door2.transform.position.y,
						door2.transform.position.z);
					ischecked = true;
				}
				if(isopen)
				{
					if(!isplayed)
					{
						GetComponent<NetworkView>().RPC("PlayDoorSound",RPCMode.All);
						isplayed = true;
					}
					door1.transform.position = new Vector3(
						door1.transform.position.x-speed,
						door1.transform.position.y,
						door1.transform.position.z);
					door2.transform.position = new Vector3(
						door2.transform.position.x+speed,
						door2.transform.position.y,
						door2.transform.position.z);
						
					if(Vector3.Distance(door1.transform.position,startposition1) < 0.2)
					{
						door1.transform.position = startposition1;
						door2.transform.position = startposition2;
						isopen = false;
						isbusy = false;
						isstanding = false;
						ischecked = false;
						isplayed = false;
					}
				}
				else
				{
					if(!isplayed)
					{
						GetComponent<NetworkView>().RPC("PlayDoorSound",RPCMode.All);
						isplayed = true;
					}
					door1.transform.position = new Vector3(
						door1.transform.position.x+speed,
						door1.transform.position.y,
						door1.transform.position.z);
					door2.transform.position = new Vector3(
						door2.transform.position.x-speed,
						door2.transform.position.y,
						door2.transform.position.z);
					if(Vector3.Distance(door1.transform.position,endposition1) < 0.2)
					{
						door1.transform.position = endposition1;
						door2.transform.position = endposition2;
						if(!isdelay)
						{
							StartCoroutine(WaitDelay());
							isdelay = true;
						}
					}
				}
			}
		}
	}
	
	[RPC]
	void PlayDoorSound()
	{
		GetComponent<AudioSource>().PlayOneShot(doorsounds);
	}
	
	[RPC]
	void MoveMetro(Vector3 pos)
	{
		gameObject.transform.position = pos;
	}
	
	IEnumerator WaitDelay()
	{
		yield return new WaitForSeconds(waittime);
		isopen = true;
		isplayed = false;
		isdelay = false;
	}
}
