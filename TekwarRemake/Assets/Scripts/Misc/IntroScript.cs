using UnityEngine;
using System.Collections;

public class IntroScript : MonoBehaviour {

	public Camera maincam;
	MovieTexture movie;
	public AudioClip menu_music;
		
	void Start ()
	{
		movie = GetComponent<Renderer>().material.mainTexture as MovieTexture;
		GetComponent<AudioSource>().clip = movie.audioClip;
		GetComponent<AudioSource>().Play();
		movie.Play();
		StartCoroutine(EndIntro());
	}
	
	void FixedUpdate()
	{
		if(Input.GetKey(KeyCode.Escape))
		{
			maincam.transform.rotation = new Quaternion(0,0,0,0);
			maincam.GetComponent<AudioSource>().clip = menu_music;
			maincam.GetComponent<AudioSource>().Play();
			StopCoroutine("EndIntro");
			Destroy(gameObject);
		}
	}
	
	IEnumerator EndIntro()
	{
		yield return new WaitForSeconds(71.0f);
		maincam.transform.rotation = new Quaternion(0,0,0,0);
		maincam.GetComponent<AudioSource>().clip = menu_music;
		maincam.GetComponent<AudioSource>().Play();
		Destroy(gameObject);
	}
}
