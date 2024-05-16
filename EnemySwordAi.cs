using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySwordAi : MonoBehaviour
{
    public ManageTarget mt;
    public NavMeshAgent agent;
    public Transform enemy;
    public string enemyTag;
    public LayerMask whatIsEnemy;
    public Animator anim;
    public TrailRenderer tr;
    public float dmg;

   
    public float timeBetweenAttacks;
    public float timer;

    public Vector3 walkPoint;
    public float walkAwayRange;
    public float attackRange;

    public bool CanHit;
    public bool walkPointSet;
    public bool playerInAttackRange;
    public bool playerInRealAttackRange;
    public bool hasAttacked;
    public AudioSource audio;
    public AudioClip SwingClip1;
    public AudioClip SwingClip2;




    // Start is called before the first frame update
    void Start()
    {
        mt = transform.parent.GetComponent<ManageTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        
           

        //cauta inamicul
        enemy = mt.FindTarget(enemyTag, transform);
      

        

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);
        playerInRealAttackRange = Physics.CheckSphere(transform.position, 1, whatIsEnemy);

       

        if (enemy != null)
        {
            

            transform.LookAt(enemy.position);

            if (timer > 0 && !CanHit)
            {
                //daca a atacat, pauza

                if (walkPointSet)
                {
                    agent.SetDestination(walkPoint);

                    float distanceToWalkPoint = Vector3.Distance(transform.position, walkPoint);


                    if (distanceToWalkPoint <= 1.25f)
                    {
                        walkPointSet = false;
                    }

                }
                else
                    SearchPoint();
            }
            else
            {
                //merge la player / ataca

                if (!playerInAttackRange)
                {
                    ChaseEnemy();
                }
                else
                    AttackEnemy();
            }

            UpdateSpeed();

        }
        else
        {
            anim.SetBool("hit1", false);
            anim.SetBool("hit2", false);
            agent.SetDestination(transform.position);
        }




       

        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("hit1") || anim.GetCurrentAnimatorStateInfo(0).IsName("hit2")) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
        {
            tr.emitting = true;
            CanHit = true;
        }
        else
        {
            tr.emitting = false;
            CanHit = false;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
           

    }

    public void ChaseEnemy()
    {
        anim.SetBool("hit1", false);
        anim.SetBool("hit2", false);
        
        if(hasAttacked)
        {
         
            timer = timeBetweenAttacks;
            hasAttacked = false;
        }
           

        agent.SetDestination(enemy.position);
    }

    public void SearchPoint()
    {
        Vector3 dirToPlayer;
        Vector3 dirToPoint;
            
        //cauta cel mai apropriat loc de navmesh

        do
        {
            float randomZ = Random.Range(-walkAwayRange, walkAwayRange);
            float randomX = Random.Range(-walkAwayRange, walkAwayRange);

            walkPointSet = true;
            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

          dirToPlayer = (enemy.position - transform.position).normalized;
          dirToPoint = (walkPoint - transform.position).normalized;


        } 
        while (Vector3.Angle(dirToPlayer, dirToPoint) < 45);

     

        NavMeshHit hit;

        if (NavMesh.SamplePosition(walkPoint, out hit, 100f, NavMesh.AllAreas))
        {
           

            if(Vector3.Distance(transform.position,enemy.position) < Vector3.Distance(hit.position, enemy.position))
                walkPoint = hit.position;

          
        }


    }


    private void AttackEnemy()
    {
       

      

        if(!playerInRealAttackRange)
            agent.SetDestination(enemy.position);
        else
            agent.SetDestination(transform.position);






        hasAttacked = true;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Sword Movement"))
            {
            //primul atac
                if (!anim.GetBool("hit1"))
                    audio.PlayOneShot(SwingClip1, 1f);

                anim.SetBool("hit1", true);
            
             }


            if (anim.GetCurrentAnimatorStateInfo(0).IsName("hit1") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
            {
             //al doilea atac
                if (!anim.GetBool("hit2"))
                    audio.PlayOneShot(SwingClip2, 1f);

                anim.SetBool("hit1", false);
                anim.SetBool("hit2", true);

       
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("hit2") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
            {
                anim.SetBool("hit2", false);

                hasAttacked = false;
                timer = timeBetweenAttacks;
            //couldown
            }

           
        


     

       
    }

   

    private void UpdateSpeed()
    {
        if (enemy == null)
            anim.SetFloat("Speed", 0f, 0.2f, Time.deltaTime);

        if (Vector3.Distance(transform.position, enemy.position) > 4 * attackRange && timer <= 0)
        {
            anim.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
            agent.speed = 6;
        }
        else if (Vector3.Distance(transform.position, enemy.position) > 3 * attackRange)
        {
            anim.SetFloat("Speed",0.5f, 0.2f, Time.deltaTime);
            agent.speed = 4;
        }


    }




}
