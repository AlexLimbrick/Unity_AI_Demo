using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSM_Skeleton : MonoBehaviour, HitMe
{
    private IEnumerator currState;
    private IEnumerator nextState;
    public GameObject player;
    //public GameObject Sensor;
    NavMeshAgent agent;
    Animator anim;
    public float health;
    bool dying = false;
    float AttackCooldown;
    RaycastHit hit;
    public void Hit(float damage)
    {
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            nextState = Dead();
            agent.SetDestination(transform.position);
        }
        else
        {
            anim.Play("Skeleton_Hit");
        }
    }
    public bool flipX
    {
        set
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
    private IEnumerator _FSM()
    {
        while(currState != null)
        {
            yield return StartCoroutine(currState);
            currState = nextState;
            nextState = null;
        }
    }
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currState = Idle();
        StartCoroutine(_FSM());
        player = GameObject.FindGameObjectWithTag("Player");
      
	}
	
	// Update is called once per frame
	void Update () {
        AttackCooldown -= Time.deltaTime;
	}
    void say(string i)
    {
        Debug.Log(i);
    }

    private IEnumerator Idle()
    {
        while(nextState == null)
        {
            say("Idle");

            Debug.DrawRay(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z));

            if (Physics.Raycast(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z), out hit))
            {
                //Debug.DrawRay(transform.position, player.transform.position - transform.position);
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject == player)
                {
                    nextState = Chase();
                }
            }
            

            if(health<= 0)
            {
                nextState = Dead();
            }
            GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(90, 0, 0);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            yield return null;
        }
    }

    private IEnumerator Chase()
    {
        while (nextState == null)
        {
            if(player.transform.position.z > transform.position.z)
            {
                GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
            else
            {
                GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            if (player.transform.position.x > transform.position.x)
            {
                flipX = false;
            }
            else
            {

                flipX = true;
            }
            if (health <= 0)
            {
                nextState = Dead();
                agent.isStopped = true;
            }
            

            say("Chase");
            agent.SetDestination(player.transform.position);
            GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(90, 0, 0);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            anim.SetFloat("Speed", agent.velocity.magnitude);
            yield return null;
        }
    }
    private IEnumerator Dead()
    {
        while (nextState == null)
        {
            GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(90, 0, 0);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            if (!dying)
            {
                anim.Play("Skeleton_Death");
                dying = true;
            }

            if (!GetComponent<SpriteRenderer>().enabled)
            {
                Destroy(gameObject);
            }

            yield return null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (health <= 0)
        {
            nextState = Dead();
            agent.SetDestination(transform.position);
        }
        else if (other.gameObject.layer == 8 && AttackCooldown <= 0)
        {
            
            agent.SetDestination(transform.position);
            anim.Play("Skeleton_Atack");
            AttackCooldown = 1;
        }
        
    }
}
