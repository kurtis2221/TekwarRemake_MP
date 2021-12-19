using UnityEngine;
using System.Collections;
using System.IO;

public class MenuScript2 : MonoBehaviour {
	
	public int funcid = 0;
	string tmp = null;
	int tmp2 = 0;
	StreamReader sr;
	
	// Use this for initialization
	void Start () {
		if(funcid == 0)
		{
			if(File.Exists("config.ini"))
			{
				sr = new StreamReader("config.ini");
				while(true)
				{
					tmp = sr.ReadLine();
					if(tmp == null) break;
					
					if(tmp.StartsWith("Name="))
					{
						GetComponent<TextMesh>().text = tmp.Replace("Name=", "");
						break;	
					}
				}
				sr.Close();
			}
		}
		if(funcid == 1)
		{
			if(File.Exists("config.ini"))
			{
				sr = new StreamReader("config.ini");
				while(true)
				{
					tmp = sr.ReadLine();
					if(tmp == null)
					{
						GetComponent<TextMesh>().text = "ON";
						break;
					}
					
					if(tmp.StartsWith("Music="))
					{
						tmp = tmp.Replace("Music=", "");
						if(tmp == "ON")
							GetComponent<TextMesh>().text = "ON";
						else
							GetComponent<TextMesh>().text = "OFF";
						break;	
					}
				}
				sr.Close();
			}
		}
		if(funcid == 2)
		{
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
						tmp2 = System.Convert.ToInt32(tmp[0].ToString());
						if(tmp2 < 0 && tmp2 > 7) tmp2 = 0;
						}
						else tmp2 = 0;
						GetComponent<TextMesh>().text = tmp2.ToString();
						break;	
					}
				}
				sr.Close();
			}
		}
	}
}
