using UnityEngine;
using System.IO;

public class GameMenu : MonoBehaviour
{
    public GameObject PlayerPrefab;
	public string player_name;
	public GameObject MetroObject;
	GameObject[] spawns;
	public GameObject pobj;
	
	public GameObject MetroSpawn;
    	string ip = "127.0.0.1";
	bool isloaded = false;
	bool metroenabled = false;
	public bool npcenabled = false;
	int crate = 20;
	int srate = 20;
	
	StreamReader sr;
	string tmp = null;

	void Start()
	{
		//GetPlayerName
		if(File.Exists("config.ini"))
		{
			sr = new StreamReader("config.ini");
			while(true)
			{
				tmp = sr.ReadLine();
				if(tmp == null) break;
					
				if(tmp.StartsWith("Name="))
				{
					player_name = tmp.Replace("Name=", "");
					break;	
				}
			}
			sr.Close();
			
			sr = new StreamReader("config.ini");
			while(true)
			{
				tmp = sr.ReadLine();
				if(tmp == null) break;
					
				if(tmp.StartsWith("Metro="))
				{
					if(tmp.Replace("Metro=", "") == "True")
						metroenabled = true;
						else
						metroenabled = false;
					break;	
				}
			}
			sr.Close();
			
			sr = new StreamReader("config.ini");
			while(true)
			{
				tmp = sr.ReadLine();
				if(tmp == null) break;
					
				if(tmp.StartsWith("NPC="))
				{
					if(tmp.Replace("NPC=", "") == "True")
						npcenabled = true;
						else
						npcenabled = false;
					break;	
				}
			}
			sr.Close();
			
			sr = new StreamReader("config.ini");
			while(true)
			{
				tmp = sr.ReadLine();
				if(tmp == null) break;
					
				if(tmp.StartsWith("ClientRate="))
				{
					tmp = tmp.Replace("ClientRate=", "");
						
					for(int i = 0; i < tmp.Length; i++)
						if(!char.IsNumber(tmp[i])) goto end;
					crate = System.Convert.ToInt32(tmp);
					break;
				}
			}
		end: {}
			sr.Close();
			
			sr = new StreamReader("config.ini");
			while(true)
			{
				tmp = sr.ReadLine();
				if(tmp == null) break;
					
				if(tmp.StartsWith("ServerRate="))
				{
					tmp = tmp.Replace("ServerRate=", "");
					
					for(int i = 0; i < tmp.Length; i++)
						if(!char.IsNumber(tmp[i])) goto end2;
					srate = System.Convert.ToInt32(tmp);
					break;	
				}
			}
		end2: {}
			sr.Close();
		}
		if(!npcenabled)
		{
			GameObject[] tmp = GameObject.FindGameObjectsWithTag("NPC");
			for(int i = 0; i < tmp.Length; i++)
			{
				Network.RemoveRPCs(tmp[i].GetComponent<NetworkView>().viewID);
				Network.Destroy(tmp[i]);
				Destroy(tmp[i]);
			}
			
		}
		//GetPlayerName end	
	}
    public void CreatePlayer()
    {
	spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
	int rnd = Random.Range(0,spawns.Length);
        connected = true;
        pobj = (GameObject)Network.Instantiate(PlayerPrefab, spawns[rnd].transform.position, spawns[rnd].transform.rotation, 0);
	if(Network.isServer && metroenabled)
	{
		Network.Instantiate(MetroObject, MetroSpawn.transform.position, MetroSpawn.transform.rotation, 1);
		MetroObject.GetComponent<MetroScript>().enabled = true;
	}
	GetComponent<Camera>().enabled = false;
	GetComponent<AudioListener>().enabled = false;
	GetComponent<NetScore>().ServAddPlayer(player_name,pobj.GetComponent<NetworkView>().viewID);
    }
	void OnPlayerConnected()
	{
		pobj.GetComponent<MainScript>().SendHudInfo("Player connected.");
	}
    void OnDisconnectedFromServer()
    {
        connected = false;
	Application.LoadLevel(1);
    }
	
    public void RemoveScore()
    {
	GetComponent<NetScore>().ServRemPlayer(player_name);
    }
	
    void OnPlayerDisconnected(NetworkPlayer pl)
    {
	GetComponent<NetScore>().ServRemPlayer(player_name);
	pobj.GetComponent<MainScript>().SendHudInfo("Player disconnected.");
	Network.RemoveRPCs(pl);
        Network.DestroyPlayerObjects(pl);
    }
    void OnConnectedToServer()
    {
        CreatePlayer();
	Network.sendRate = crate;
    }
    void OnServerInitialized()
    {
        CreatePlayer();
	GetComponent<NetScore>().StartScanning();
	Network.sendRate = srate;
    }
    bool connected;
    void OnGUI()
    {
        if (!connected)
        {
            ip = GUI.TextField(new Rect(Screen.width / 2 -64,Screen.height /2,128,24),ip);
			if(File.Exists("lastaddr.txt") && !isloaded)
			{
				StreamReader sr = new StreamReader("lastaddr.txt");
				ip = sr.ReadToEnd();
				sr.Close();
				isloaded = true;
			}
            if (GUI.Button(new Rect(Screen.width / 2 -64,Screen.height /2 + 24,128,24),"Connect"))
            {
                Network.Connect(ip, 5300);
				FileStream fs = new FileStream(
					"lastaddr.txt",
					File.Exists("lastaddr.txt") ? FileMode.Truncate : FileMode.CreateNew,
					FileAccess.Write);			
				StreamWriter sw = new StreamWriter(fs);
				sw.Write(ip);
				sw.Close();
				fs.Close();
            }
            if (GUI.Button(new Rect(Screen.width / 2 -64,Screen.height /2 + 48,128,24),"Host"))
            {
                Network.InitializeServer(10, 5300, false);
            }
        }
    }
}