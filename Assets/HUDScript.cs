using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour {

    public Image Heart1;
    public Image Heart2;
    public Image Heart3;
    public Image Heart4;
    public Image Heart5;
    public Image Heart6;
    public Image Heart7;
    public Image Heart8;
    public Image Heart9;
    public Image Heart10;

    public Sprite Full;
    public Sprite Empty;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Health >= 1)
        {
            Heart1.sprite = Full; 
        }
        else
        {
            Heart1.sprite = Empty;
        }
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Health >= 2)
        {
            Heart2.sprite = Full;
        }
        else
        {
            Heart2.sprite = Empty;
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Health >= 3)
        {
            Heart3.sprite = Full;
        }
        else
        {
            Heart3.sprite = Empty;
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Health >= 4)
        {
            Heart4.sprite = Full;
        }
        else
        {
            Heart4.sprite = Empty;
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Health >= 5)
        {
            Heart5.sprite = Full;
        }
        else
        {
            Heart5.sprite = Empty;
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Health >= 6)
        {
            Heart6.sprite = Full;
        }
        else
        {
            Heart6.sprite = Empty;
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Health >= 7)
        {
            Heart7.sprite = Full;
        }
        else
        {
            Heart7.sprite = Empty;
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Health >= 8)
        {
            Heart8.sprite = Full;
        }
        else
        {
            Heart8.sprite = Empty;
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Health >= 9)
        {
            Heart9.sprite = Full;
        }
        else
        {
            Heart9.sprite = Empty;
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Health == 10)
        {
            Heart10.sprite = Full;
        }
        else
        {
            Heart10.sprite = Empty;
        }


    }
}
