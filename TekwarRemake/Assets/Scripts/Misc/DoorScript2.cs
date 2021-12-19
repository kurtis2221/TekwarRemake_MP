using UnityEngine;
using System.Collections;

public class DoorScript2 : MonoBehaviour {

	GameObject playerobj;
	public float targetX;
	public float trigdistance;
	public float movesize;
	public AudioClip open_sound;
	public AudioClip close_sound;
	
	Vector3 startposition;
	Vector3 endposition;
	Vector3 targetposition;
	
	bool iskeydown = false;
	bool isbusy = false;
	bool isadd = false;
	bool isopen = false;
	
	// Use this for initialization
	void Start () {
		startposition = gameObject.transform.position;
		endposition = new Vector3(gameObject.transform.position.x+targetX,gameObject.transform.position.y,gameObject.transform.position.z);
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
				endposition = new Vector3(gameObject.transform.position.x+targetX,gameObject.transform.position.y,gameObject.transform.position.z);
			}
			
			if(isbusy)
			{
				if(isadd && targetposition.x >= gameObject.transform.position.x)
					gameObject.transform.position = new Vector3(
						gameObject.transform.position.x+movesize,
						gameObject.transform.position.y,
						gameObject.transform.position.z);
				else if(!isadd && targetposition.x <= gameObject.transform.position.x)
					gameObject.transform.position = new Vector3(
						gameObject.transform.position.x-movesize,
						gameObject.transform.position.y,
						gameObject.transform.position.z);
				else
				{
					gameObject.transform.position = targetposition;
					if(isopen) isopen = false;
					else isopen = true;
					isbusy = false;
				}
			}
			else if(Vector3.Distance(playerobj.transform.position,startposition) <= trigdistance)
			{
				if(Input.GetButtonDown("Use") && !iskeydown)
				{	
					if(Vector3.Equals(gameObject.transform.position,startposition))
					{
						targetposition = endposition;
						GetComponent<AudioSource>().PlayOneShot(open_sound);
						isbusy = true;
						if(targetposition.x >= gameObject.transform.position.x) isadd = true;
						if(targetposition.x <= gameObject.transform.position.x) isadd = false;
					}
					else
					{
						targetposition = startposition;
						GetComponent<AudioSource>().PlayOneShot(close_sound);
						isbusy = true;
						if(targetposition.x >= gameObject.transform.position.x) isadd = true;
						if(targetposition.x <= gameObject.transform.position.x) isadd = false;
					}
					iskeydown = true;
				}
				else iskeydown = false;
			}
		}
	}
}