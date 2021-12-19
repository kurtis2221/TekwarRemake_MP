using UnityEngine;
using System.Collections;

public class gun3_ammo : MonoBehaviour {
	
	public GUIText hud_info;
	GameObject playerobj;
	public AudioClip pick_snd;

    void FixedUpdate()
    {
		if(playerobj == null)
			playerobj = GameObject.Find("Player(Clone)");
		else
		{
			if(Vector3.Distance(transform.position,playerobj.transform.position) < 2.0f &&
				playerobj.GetComponent<NetworkView>().isMine && gameObject.GetComponent<Renderer>().enabled)
			{
				if(playerobj.GetComponentInChildren<WeaponScript>().ammo[2] < 100)
				{
				hud_info.enabled = true;
				hud_info.text = "SHRIKE CHARGES";
				playerobj.GetComponent<AudioSource>().PlayOneShot(pick_snd);
				playerobj.GetComponentInChildren<WeaponScript>().ammo[2] = 100;
				playerobj.GetComponent<MainScript>().count = 3;
				StartCoroutine(Respawn());
				gameObject.GetComponent<Renderer>().enabled = false;
				}
			}
		}
	}
	
	IEnumerator Respawn()
	{
		yield return new WaitForSeconds(90.0f);
		gameObject.GetComponent<Renderer>().enabled = true;
	}
}
