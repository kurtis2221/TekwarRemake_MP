using UnityEngine;
using System.Collections;

public class GrenadeScript : MonoBehaviour {
	
	public AudioClip snd_explosion;
	
	void Start () {
		GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3 (0, 0, 30));
		GetComponent<Rigidbody>().angularVelocity = transform.TransformDirection(new Vector3(25, 0, 0));
		StartCoroutine(Explosion());
	}
	
	IEnumerator Explosion()
	{
		yield return new WaitForSeconds(3.0f);
		GetComponent<Renderer>().enabled = false;
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<AudioSource>().PlayOneShot(snd_explosion);
		GetComponent<Light>().enabled = true;
		StartCoroutine(LightOff());
		StartCoroutine(Clean());
		Collider[] other = Physics.OverlapSphere(transform.position,10.0f);
		for(int i = 0; i < other.Length; i++)
		{
			if(other[i].name.Contains("Player") && other[i].GetComponent<NetworkView>().isMine && GetComponent<NetworkView>().isMine)
				other[i].GetComponent<MainScript>().hp = 0;
			else if(other[i].name.Contains("Player") && !other[i].GetComponent<NetworkView>().isMine)
				other[i].GetComponent<Collider>().GetComponent<NetHit>().SendHit(100, 20, 35,
					other[i].GetComponent<Collider>().GetComponent<NetworkView>().viewID,gameObject.GetComponent<NetworkView>().owner,false);
			else if(other[i].name == "cop")
				other[i].GetComponent<cop_nav>().DeathTrigger();
			else if(other[i].name == "civilian")
				other[i].GetComponent<civil_nav>().DeathTrigger();
			else if(other[i].name == "ebarrel")
				other[i].GetComponent<BarrelScript>().Explode();
			else if(other[i].name == "glass")
				other[i].GetComponent<GlassScript>().Break();
		}
	}
	
	IEnumerator LightOff()
	{
		yield return new WaitForSeconds(0.5f);
		GetComponent<Light>().enabled = false;
	}
	
	IEnumerator Clean()
	{
		if(GetComponent<NetworkView>().isMine)
		{
		yield return new WaitForSeconds(2.5f);
		Network.RemoveRPCs(gameObject.GetComponent<NetworkView>().viewID);
		Network.Destroy(gameObject);
		Destroy(gameObject);
		}
	}
}
