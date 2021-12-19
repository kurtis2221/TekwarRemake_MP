using UnityEngine;
using System.Collections;
using System.IO;

public class MainScript : MonoBehaviour {
	
	public GameObject MainCam;
	public GameObject Skin;
	public GameObject[] Trash;
	public bool isplaying = false;
	bool[] isdown = new bool[6];
	GUIText hud_info;
	GUITexture crosshair;
	GUIText hud_score;
	public bool allowshoot = false;
	
	GameObject gamecontrol;
	
	public AudioClip[] track;
	int currclip = 0;
	
	//HP System
	public int hp = 100;
	public int cos = 100;
	
	//Quit to menu vars
	bool quiting = false;
	
	//File config
	string tmp = null;
	StreamReader sr;
	public int count = 0;
	
    void OnApplicationFocus(bool stat)
    {
        if(!Cursor.visible && stat)
            Screen.lockCursor = true;
    }

	// Use this for initialization
	void Start () {
		if(GetComponent<NetworkView>().isMine)
		{	
			StartCoroutine(HideMessage());
			gamecontrol = GameObject.Find("GameControl");
			if(Network.isClient)
				gamecontrol.GetComponent<SyncStart>().SendSyncCall(GetComponent<NetworkView>().viewID);
			
			GetComponent<FPSInputController>().enabled = true;
			GetComponent<CharacterMotor>().enabled = true;
			GetComponent<MouseLook>().enabled = true;
			GetComponentInChildren<MouseLook>().enabled = true;
			GetComponentInChildren<WeaponScript>().enabled = true;
			GetComponent<Validation>().enabled = true;
			for(int i = 0; i < Trash.Length; i++)
			Trash[i].layer = 9;
			GetComponent<Validation>().SetPlayer();
			GetComponent<WaterScript>().enabled = true;
			GetComponent<NetHit>().enabled = true;
			Cursor.visible = false;
			Screen.lockCursor = true;
			//CFG - Get Music Stat
			if(File.Exists("config.ini"))
			{
				sr = new StreamReader("config.ini");
				while(true)
				{
					tmp = sr.ReadLine();
					if(tmp == null)
					{
						GetComponent<TextMesh>().text = "1";
						break;
					}
						
					if(tmp.StartsWith("Track="))
					{
						tmp = tmp.Replace("Track=", "");
						if(char.IsNumber(tmp[0]))
						{
							currclip = System.Convert.ToInt32(tmp[0].ToString());
							if(currclip < 0 && currclip > 7) currclip = 0;
						}
						else currclip = 0;
						MainCam.GetComponent<AudioSource>().clip = track[currclip];
						break;	
					}
				}
				sr.Close();
				
				sr = new StreamReader("config.ini");
				while(true)
				{
					tmp = sr.ReadLine();
					if(tmp == null)
					{
						MainCam.GetComponent<AudioSource>().Play();
						isplaying = true;
						break;
					}
					
					if(tmp.StartsWith("Music="))
					{
						tmp = tmp.Replace("Music=", "");
						if(tmp == "ON")
						{
							MainCam.GetComponent<AudioSource>().Play();
							isplaying = true;
						}
						break;	
					}
				}
				sr.Close();
			}
		}
		else
		{
			GetComponent<MainScript>().enabled = false;
			GetComponent<FPSInputController>().enabled = false;
			GetComponent<CharacterMotor>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
			GetComponentInChildren<WeaponScript>().enabled = false;
			GetComponent<Validation>().enabled = false;
			GetComponent<WaterScript>().enabled = false;
			Destroy(MainCam);
		}
		hud_info = GameObject.Find("HUDInfo").GetComponent<GUIText>();
		crosshair = GameObject.Find("HudCrosshair").GetComponent<GUITexture>();
		hud_score = GameObject.Find("HudPlayers").GetComponent<GUIText>();
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
		if(Input.GetKey(KeyCode.Escape))
		{
			if(!isdown[0])
			{
			count = 60;
			hud_info.enabled = true;
			hud_info.text = "ABORT MISSION? (Y/N)";
			quiting = true;
			isdown[0] = true;
			}
		}
		else if(quiting)
		{
			if(Input.GetKey(KeyCode.Y))
			{
				gamecontrol.GetComponent<GameMenu>().RemoveScore();
				Network.Disconnect();
				Application.LoadLevel(1);
			}
			else if(Input.GetKey(KeyCode.N))
			{
				count = 0;
				hud_info.enabled = false;
				quiting = false;
			}
		}
		else isdown[0] = false;
		
		if(Input.GetButton("Music"))
		{
			if(isplaying && !isdown[1])
			{
				MainCam.GetComponent<AudioSource>().Stop();
				isplaying = false;
				isdown[1] = true;
			}
			else if(!isplaying && !isdown[1])
			{
				MainCam.GetComponent<AudioSource>().Play();
				isplaying = true;
				isdown[1] = true;
			}
		}
		else isdown[1] = false;
		
		if(Input.GetButton("Change Track"))
		{
			if(!isdown[2])
			{
			if(currclip == track.Length-1) currclip = 0;
			else currclip += 1;
			
			MainCam.GetComponent<AudioSource>().clip = track[currclip];
			MainCam.GetComponent<AudioSource>().Play();
			isplaying = true;
			isdown[2] = true;
			}
		}
		else isdown[2] = false;
		
		if(Input.GetButton("Crosshair"))
		{
			if(!isdown[3])
			{
			crosshair.enabled = !crosshair.enabled;
			isdown[3] = true;
			}
		}
		else isdown[3] = false;
		
		if(Input.GetButton("ScoreBoard"))
		{
			if(!isdown[4])
			{
			hud_score.enabled = !hud_score.enabled;
			isdown[4] = true;
			}
		}
		else isdown[4] = false;
		
		if(Input.GetButton("Talk"))
		{
			if(!isdown[5])
				isdown[5] = true;
		}
		else if(!Input.GetButton("Talk") && isdown[5])
		{
			gamecontrol.GetComponent<ChatScript>().enabled = true;
			isdown[5] = false;
		}
		
		if(!allowshoot)
		{
			if(Input.GetButton("Run/Sprint") &&
				(Input.GetButton("Horizontal") || Input.GetButton("Vertical")))			
				Skin.GetComponent<NetAnim>().PlayAnimation(Skin.GetComponent<NetworkView>().viewID,"run");
			else if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
				Skin.GetComponent<NetAnim>().PlayAnimation(Skin.GetComponent<NetworkView>().viewID,"walk");
			else
				Skin.GetComponent<NetAnim>().PlayAnimation(Skin.GetComponent<NetworkView>().viewID,"idle");
		}
	}
	
	public void SendHudInfo(string message)
	{
		hud_info.enabled = true;
		hud_info.text = message;
		count = 3;
	}
	
	IEnumerator HideMessage()
	{
		while(true)
		{
			yield return new WaitForSeconds(1.0f);
			if(count > 0) count-=1;
			else if(count == 0 && hud_info.enabled) hud_info.enabled = false;
		}
	}
}
