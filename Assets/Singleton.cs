using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour {

    public static Singleton instance;
    public float Health;
    public string LastLevel;
    // Use this for initialization
    void Awake () {
		
        if (instance && this != instance)
        {
            Destroy(gameObject);

            return;
        }
        Health = 10;
        instance = this;
        
        DontDestroyOnLoad(gameObject);
	}
}
