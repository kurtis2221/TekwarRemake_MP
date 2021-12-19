using UnityEngine;
using System.Collections;

public class HPScript : MonoBehaviour {
	
	GameObject[] spawns;
	int rnd;
	public AudioClip death_snd;
	GameObject playerobj;
	float tmp;
	float hp;
	float hp_check;
	float hp_max;
	
	void Start()
	{
		hp_max = GetComponent<GUITexture>().pixelInset.width;
		spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
	}
	
	// Update is called once per frame
    	void FixedUpdate()
    	{
		if(playerobj == null)
			playerobj = GameObject.Find("Player(Clone)");
		else if(playerobj != null)
		{
			hp = playerobj.GetComponent<MainScript>().hp;
			if(hp_check != hp)
			{
			tmp = hp / 100 * hp_max;
			GetComponent<GUITexture>().pixelInset = new Rect(
				GetComponent<GUITexture>().pixelInset.x,
				GetComponent<GUITexture>().pixelInset.y,
				tmp,
				GetComponent<GUITexture>().pixelInset.height);
			}
			else if(hp <= 0)
			{
				rnd = Random.Range(0,spawns.Length);
				playerobj.transform.position = spawns[rnd].transform.position;
				//playerobj.transform.rotation = spawns[rnd].transform.rotation;
				playerobj.GetComponent<AudioSource>().PlayOneShot(death_snd);
				playerobj.GetComponentInChildren<NetSnd>().RecSnd(playerobj.GetComponent<NetworkView>().viewID,3);
				playerobj.GetComponent<MainScript>().hp = 100;
				playerobj.GetComponent<MainScript>().cos = 100;
				for(int i = 0; i < 3; i++)
					playerobj.GetComponentInChildren<WeaponScript>().ammo[i] = 100;
				playerobj.GetComponentInChildren<WeaponScript>().gren_ammo = 3;
				GetComponent<GUITexture>().pixelInset = new Rect(
					GetComponent<GUITexture>().pixelInset.x,
					GetComponent<GUITexture>().pixelInset.y,
					hp_max,
					GetComponent<GUITexture>().pixelInset.height);
			}
			hp_check = playerobj.GetComponent<MainScript>().hp;
		}
	}
}
