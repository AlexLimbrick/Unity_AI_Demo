using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour {
    public string Level;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            Singleton.instance.Health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Health;
            SwitchLevel(Level);
        }
    }

    public void SwitchLevel(string _Level)
    {
        SceneManager.LoadScene(_Level, LoadSceneMode.Single);
    }
    public void LastLevel()
    {
        SceneManager.LoadScene(Singleton.instance.LastLevel, LoadSceneMode.Single);
    }
    public void quitgame()
    {
        Application.Quit();
    }
}
