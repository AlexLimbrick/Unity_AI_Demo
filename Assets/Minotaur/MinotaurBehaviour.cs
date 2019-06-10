using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class MinotaurBehaviour
    : MonoBehaviour, HitMe
{
    private IEnumerator currState;
    private IEnumerator nextState;
    public GameObject player;
    public Object Skeleton;
    public Object Wizard;

    //public GameObject Sensor;
    NavMeshAgent agent;
    Animator anim;
    public float maxHealth;
    public float health;
   
    float AttackCooldown;
    RaycastHit hit;
    float DistanceFromPlayer;
    float comboCooldown;
    List<GameObject> summonAreas;
    float SummonTime = 3;
    float SummonCooldown;
    float wait;
    [SerializeField]float summonChance;

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
        while (currState != null)
        {
            yield return StartCoroutine(currState);
            currState = nextState;
            nextState = null;
        }
    }
    public void Hit(float damage)
    {
        health -= damage;
        anim.Play("MinotaurInjured1");
        nextState = Idle();
        SummonTime = 3;
        anim.SetInteger("Summoning", 0);
    }
    // Use this for initialization
    void Start()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        summonAreas = new List<GameObject>();
        summonAreas.AddRange(GameObject.FindGameObjectsWithTag("SummonZone"));
        currState = Idle();
        StartCoroutine(_FSM());
    }
    void WinGame()
    {
        Singleton.instance.Health = 10;
        SceneManager.LoadScene("WinScreen", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {

        Senses();
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(90, 0, 0);
        anim.SetFloat("Speed", agent.velocity.magnitude);
        comboCooldown -= Time.deltaTime;
        SummonCooldown -= Time.deltaTime;
        summonChance = (health / maxHealth)/1.5f;
       
            if (Random.Range(0, 0.9f) > summonChance && SummonCooldown <= 0)
            {
                nextState = SummonAllies();
            }

        
        if (player.transform.position.z > transform.position.z)
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

    }
    void Senses()
    {
        Physics.Raycast(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z), out hit);
        DistanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        if(Physics.Raycast(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z), out hit))
        {
            if (DistanceFromPlayer < 5 && hit.collider.gameObject == player && comboCooldown <= 0 && currState != SummonAllies()&&nextState != SummonAllies())
            {
                nextState = Attack();
            }
           
        }
        
    }

    private IEnumerator Idle()
    {
        while (nextState == null)
        {
            Debug.DrawRay(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z));

            if (Physics.Raycast(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z), out hit))
            {
                //Debug.DrawRay(transform.position, player.transform.position - transform.position);
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject == player)
                {
                    nextState = Chase();
                }
                if (health <= 0)
                {
                    nextState = Dead();
                }
                GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(90, 0, 0);
                
            }
            yield return null;
        }

    }
    private IEnumerator Chase()
    {
        while (nextState == null)
        {
            if(!(DistanceFromPlayer < 5))
            {
                agent.isStopped = false;
            }
            
            agent.SetDestination(player.transform.position);
            yield return null;
        }
    }
    private IEnumerator SummonAllies()
    {
        while (nextState == null)
        {
            agent.isStopped = true;
            if (SummonTime == 3)
            {
                anim.SetInteger("Summoning", 1);
            }

            SummonTime -= Time.deltaTime;
            if (SummonTime <= 0)
            {
                int summonNo = Random.Range(1, 3);
                for (int i = 0; i < summonNo; i++)
                {
                    if(Random.Range(0,4)>1)
                    {
                        Instantiate(Skeleton, summonAreas[Random.Range(0, summonAreas.Count - 1)].transform);
                    }
                    else
                    {
                        Instantiate(Wizard, summonAreas[Random.Range(0, summonAreas.Count - 1)].transform);
                    }
                   
                    
                }
                nextState = Idle();
                SummonTime = 3;
                SummonCooldown = 10;
                anim.SetInteger("Summoning", 0);
            }
            
            
             
            yield return null;
        }
    }
    private IEnumerator Attack()
    {
        while (nextState == null)
        {
            agent.isStopped = true;
            anim.Play("MinotaurAttack");
            
           
            nextState = Idle();
            comboCooldown = 3;

            yield return null;
        }
    }
    private IEnumerator Dead()
    {
        while (nextState == null)
        {
            anim.Play("MinotaurDeath");
            yield return null;
        }
    }
}