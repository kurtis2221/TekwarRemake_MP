using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {
	
	public GameObject mainplayer;
	public GameObject playerobj;
	
	public GameObject handobj;
	public GameObject weaponobj1;
	public GameObject weaponobj2;
	public GameObject weaponobj3;
	
	public Transform aimobject;
	
	public Rigidbody proj1;
	public Rigidbody proj2;
	public Rigidbody proj3;
	
	public Rigidbody gren;
	
	public AudioClip sound1;
	public AudioClip sound2;
	public AudioClip sound3;
	
	public int currweap;
	int tmpweap;
	public int[] ammo = new int[3];
	public int gren_ammo = 3;
	bool isdown = false;
	
	Quaternion weapon_start;
	Quaternion weapon_end;
	public bool blockshot = false;
	public bool blockthrow = false;
	
	void Start()
	{
		weapon_start =
		new Quaternion(handobj.transform.localRotation.x,
						handobj.transform.localRotation.y,
						handobj.transform.localRotation.z,
						handobj.transform.localRotation.w);
		weapon_end =
		new Quaternion(handobj.transform.localRotation.x,
						handobj.transform.localRotation.y,
						handobj.transform.localRotation.z+0.05f,
						handobj.transform.localRotation.w);
		for(int i = 0; i < 3; i++)
			ammo[i] = 100;
		weaponobj1.GetComponent<Renderer>().enabled = false;
		weaponobj2.GetComponent<Renderer>().enabled = true;
		weaponobj3.GetComponent<Renderer>().enabled = false;
		currweap = 2;
		StartCoroutine(RechargeStun());
	}
	
	bool rnd(int percent)
        {
            if (Random.Range(0,101) <= percent) return true;
            else return false;
        }
	
	void ShootGun(Rigidbody projectile, AudioClip weapsnd, int sndid, int reduce)
	{
		if(ammo[currweap-1] > 0 && rnd(mainplayer.GetComponent<MainScript>().cos+20))
		{
			ammo[currweap-1] -= reduce;
			playerobj.GetComponent<NetAnim>().PlayAnimation(playerobj.GetComponent<NetworkView>().viewID,"shoot");
			playerobj.GetComponent<AudioSource>().PlayOneShot(weapsnd);
			playerobj.GetComponent<NetSnd>().RecSnd(playerobj.GetComponent<NetworkView>().viewID,sndid);
			Network.Instantiate(projectile, aimobject.transform.position, aimobject.transform.rotation,2);		
			handobj.transform.localRotation = weapon_end;
			blockshot = true;
			mainplayer.GetComponent<MainScript>().allowshoot = true;
			StartCoroutine(ResetHand());
			Collider[] other = Physics.OverlapSphere(transform.position,32.0f);
			for(int i = 0; i < other.Length; i++)
			{
				if(other[i].gameObject.name == "cop")
					other[i].GetComponent<cop_nav>().LookTrigger(mainplayer);
				else if(other[i].gameObject.name == "civilian")
					other[i].GetComponent<civil_nav>().LookTrigger(mainplayer);
			}
		}
	}
	
	void ChangeWeapon(int weapid)
	{
		if(currweap != weapid)
		{
			handobj.GetComponent<Renderer>().enabled = false;
			weaponobj1.GetComponent<Renderer>().enabled = false;
			weaponobj2.GetComponent<Renderer>().enabled = false;
			weaponobj3.GetComponent<Renderer>().enabled = false;
			currweap = weapid;
			tmpweap = currweap;
			blockshot = true;
			StartCoroutine(ShowHand());
		}
	}
	
	IEnumerator RechargeStun()
	{
		yield return new WaitForSeconds(1.0f);
		if(ammo[0] < 100)
			ammo[0] += 10;
		StartCoroutine(RechargeStun());
	}
	
	IEnumerator ResetHand()
	{
		yield return new WaitForSeconds(0.25f);
		handobj.transform.localRotation = weapon_start;
		blockshot = false;
		mainplayer.GetComponent<MainScript>().allowshoot = false;
	}
	
	IEnumerator ShowHand()
	{
		yield return new WaitForSeconds(0.5f);
		if(currweap == 1)
		{
			handobj.GetComponent<Renderer>().enabled = true;
			weaponobj1.GetComponent<Renderer>().enabled = true;
			weaponobj2.GetComponent<Renderer>().enabled = false;
			weaponobj3.GetComponent<Renderer>().enabled = false;
			currweap = 1;	
		}
		if(currweap == 2)
		{
			handobj.GetComponent<Renderer>().enabled = true;
			weaponobj1.GetComponent<Renderer>().enabled = false;
			weaponobj2.GetComponent<Renderer>().enabled = true;
			weaponobj3.GetComponent<Renderer>().enabled = false;
			currweap = 2;	
		}
		if(currweap == 3)
		{
			handobj.GetComponent<Renderer>().enabled = true;
			weaponobj1.GetComponent<Renderer>().enabled = false;
			weaponobj2.GetComponent<Renderer>().enabled = false;
			weaponobj3.GetComponent<Renderer>().enabled = true;
			currweap = 3;
		}
		blockshot = false;
	}
	
	IEnumerator GrenWait()
	{
		yield return new WaitForSeconds(2.0f);
		blockthrow = false;
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
		if(Input.GetButton("Fire1") && !isdown && !blockshot)
		{
			if(currweap == 1)
				ShootGun(proj1, sound1, 0, 10);
			else if(currweap == 2)
				ShootGun(proj2, sound2, 1, 5);
			else if(currweap == 3)
				ShootGun(proj3, sound3, 2, 10);
			isdown = true;
		}
		else if(!Input.GetButton("Fire1") && isdown) isdown = false;
		else if(Input.GetKey(KeyCode.Alpha0))
		{
			handobj.GetComponent<Renderer>().enabled = false;
			weaponobj1.GetComponent<Renderer>().enabled = false;
			weaponobj2.GetComponent<Renderer>().enabled = false;
			weaponobj3.GetComponent<Renderer>().enabled = false;
			currweap = 0;
		}
		else if(Input.GetKey(KeyCode.Alpha1))
		{
			ChangeWeapon(1);
		}
		else if(Input.GetKey(KeyCode.Alpha2))
		{
			ChangeWeapon(2);
		}
		else if(Input.GetKey(KeyCode.Alpha3))
		{
			ChangeWeapon(3);
		}
		else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0)
		{
			tmpweap += 1;
			if(tmpweap > 3) tmpweap = 1;
			ChangeWeapon(tmpweap);
		}
		else if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
		{
			tmpweap -= 1;
			if(tmpweap < 1) tmpweap = 3;
			ChangeWeapon(tmpweap);
		}
		else if(Input.GetButton("Grenade") && !isdown && !blockthrow)
		{
			if(gren_ammo > 0)
			{
				gren_ammo-=1;
				Network.Instantiate(gren, aimobject.transform.position, aimobject.transform.rotation,2);
			}
			else
			{
				GameObject.Find("HudGren").GetComponent<HUDGren2>().ShowInfo();
				GameObject.Find("HudGrenNumb").GetComponent<HUDGren>().ShowInfo();
			}
			blockthrow = true;
			StartCoroutine(GrenWait());
		}
	}
}
