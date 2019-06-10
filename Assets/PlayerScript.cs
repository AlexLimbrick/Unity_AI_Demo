using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerScript : MonoBehaviour,HitMe {

    // Use this for initialization
    Animator anim;
    float speed;
    public float Health;
    float AttackCooldown;
    public bool flipX { set
        {
            var scale = transform.localScale;
            scale.x = value ? -1f : 1f;
            transform.localScale = scale;
        }
        get
        {
            return transform.localScale.x < 0;
        }
    }

    public void Hit(float damage)
    {
        Health -= damage;
        anim.SetTrigger("Hit");
        if(Health<=0)
        {
            Singleton.instance.Health = 10;
            Singleton.instance.LastLevel = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("DeadScreen", LoadSceneMode.Single);
        }
    }

    void Start () {
        anim = GetComponent<Animator>();
        if(Singleton.instance !=null)
        {
            Health = Singleton.instance.Health;
        }
        else
        {
            Health = 10;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        AttackCooldown -= Time.deltaTime;
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0).normalized * Time.deltaTime*2.5f );
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        speed = Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));
        anim.SetFloat("Speed", speed);
        if(Input.GetAxis("Horizontal") <0)
        {
           flipX = true;
            
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            flipX = false;
           
        }
       
        if(Input.GetAxis("Fire1")>0 &&AttackCooldown<=0)
        {
            anim.Play("Attack");
            AttackCooldown = 1;
        }
       
    }
}
