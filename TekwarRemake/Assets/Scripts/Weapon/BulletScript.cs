using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	void Start()
	{
		GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3 (0, 0, 100));
		GetComponentInChildren<Renderer>().enabled = false;
		if(GetComponent<NetworkView>().isMine) ShootBullet();
		else enabled = false;
		StartCoroutine(ShowUp());
		StartCoroutine(DestroyAuto());
	}
	
	public AudioClip impact_sound;
	Ray ray;
	RaycastHit hit = new RaycastHit();
	bool hit_before = false;
	public GameObject impact_particle;
	
	void FixedUpdate()
	{
		ShootBullet();
	}
	
	void ShootBullet()
	{
		if(!hit_before)
		{
			ray = new Ray(transform.position,transform.forward);
			Physics.SphereCast(ray,0.5f,out hit,2.0f);
			if(hit.collider)
			{
				if(hit.collider.gameObject.name.Contains("Player"))
				{
					if(!hit.collider.gameObject.GetComponent<NetworkView>().isMine && gameObject.GetComponent<NetworkView>().isMine)
					{
						hit.collider.GetComponent<NetHit>().SendHit(10, 20, 35,
							hit.collider.GetComponent<NetworkView>().viewID,gameObject.GetComponent<NetworkView>().owner,false);
						InitDestroy();
					}
				}
				else if(hit.collider.gameObject.name == "civilian")
				{
					hit.collider.GetComponent<civil_nav>().DeathTrigger();
					InitDestroy();
				}
				else if(hit.collider.gameObject.name == "cop")
				{
					hit.collider.GetComponent<cop_nav>().DeathTrigger();
					InitDestroy();
				}
				else
					InitDestroy();
			}
		}
	}
	
	void InitDestroy()
	{
		Network.Instantiate(impact_particle,transform.position,transform.rotation,2);
		GetComponent<AudioSource>().PlayOneShot(impact_sound);
		hit_before = true;
		GetComponentInChildren<Renderer>().enabled = false;
		StartCoroutine(DestroyBullet());	
	}
	
	IEnumerator DestroyBullet()
	{
		if(GetComponent<NetworkView>().isMine)
		{
		yield return new WaitForSeconds(0.5f);
		Network.RemoveRPCs(gameObject.GetComponent<NetworkView>().viewID);
		Network.Destroy(gameObject);
		Destroy(gameObject);
		}
	}
	
	IEnumerator DestroyAuto()
	{
		if(GetComponent<NetworkView>().isMine)
		{
		yield return new WaitForSeconds(3);
		Network.RemoveRPCs(gameObject.GetComponent<NetworkView>().viewID);
		Network.Destroy(gameObject);
		Destroy(gameObject);
		}
	}
	
	IEnumerator ShowUp()
	{
		yield return new WaitForSeconds(0.04f);
		GetComponentInChildren<Renderer>().enabled = true;
	}
}