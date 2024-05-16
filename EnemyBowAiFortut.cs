using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBowAiFortut : MonoBehaviour
{
    public ManageTarget mt;
    public NavMeshAgent agent;
    public Transform enemy;
    public string enemyTag;
    public LayerMask whatIsEnemy;
    public Animator anim;
    public Animator bowAnim;

    public float reloadTime;
    public arrowForTut arrowPrefab;
    public Transform spawnArrow;
    public arrowForTut currentArrow;
    public bool isReloading;

    public float timeBetweenAttacks;
    public float timeBeforeShooting;
    public float timer;
    public float timer2;

    public Vector3 walkPoint;
    public float walkAwayRange;
    public float attackRange;
    public bool walkPointSet;
    public bool playerInAttackRange;
    public bool playerInRealAttackRange;
    public AudioSource audio;
    public AudioClip fireSound;
    public AudioClip pullSound;

    public float firePower;
    public float Basedmg;

    // Start is called before the first frame update
    void Start()
    {
        mt = transform.parent.GetComponent<ManageTarget>();
        Reload();

    }

    // Update is called once per frame
    void Update()
    {
        enemy = mt.FindTarget(enemyTag, transform);

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);
        playerInRealAttackRange = Physics.CheckSphere(transform.position, attackRange - 1, whatIsEnemy);

        if (enemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.position);



            Vector3 down = new Vector3(0, distanceToEnemy / attackRange * 1.5f, 0);

       

            transform.LookAt(enemy.position - down);

            if (timer > 0)
            {


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



                if (!playerInAttackRange)
                {
                    ChaseEnemy();
                }
                else
                {


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


                    AttackEnemy();
                }

            }

            UpdateSpeed();

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }

            if (timer2 > 0)
            {
                timer2 -= Time.deltaTime;
            }
        }
        else
        {
            agent.SetDestination(transform.position);
            anim.SetBool("isPullingBow", false);
            bowAnim.SetBool("isPulling", false);
        }
    }

    public void ChaseEnemy()
    {
        anim.SetBool("isPullingBow", false);
        bowAnim.SetBool("isPulling", false);
        timer2 = 0;
        walkPointSet = false;


        agent.SetDestination(enemy.position);
    }

    public void SearchPoint()
    {
        Vector3 dirToPlayer;
        Vector3 dirToPoint;

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


            if (Vector3.Distance(transform.position, enemy.position) < Vector3.Distance(hit.position, enemy.position))
                walkPoint = hit.position;


        }


    }

    public void AttackEnemy()
    {

        if (!playerInRealAttackRange)
            agent.SetDestination(enemy.position);
        else
        {
            if (!walkPointSet)
                agent.SetDestination(transform.position);
        }





        if (!anim.GetBool("isPullingBow"))
        {
            anim.SetBool("isPullingBow", true);
            bowAnim.SetBool("isPulling", true);
            timer2 = timeBeforeShooting;
            audio.PlayOneShot(pullSound, 1f);
        }
        else if (timer2 <= 0)
        {
            bowAnim.SetBool("shot", true);
            anim.SetBool("shotBow", true);

            bowAnim.SetBool("isPulling", false);
            anim.SetBool("isPullingBow", false);
            timer = timeBetweenAttacks;

            Fire();
            audio.PlayOneShot(fireSound, 1f);
        }





    }

    public void Reload()
    {
        if (isReloading || currentArrow != null)
            return;

        isReloading = true;

        StartCoroutine(ReloadAfterTime());
    }

    public IEnumerator ReloadAfterTime()
    {
        yield return new WaitForSeconds(reloadTime);
        currentArrow = Instantiate(arrowPrefab, spawnArrow);
        currentArrow.transform.localPosition = Vector3.zero;

        isReloading = false;
    }

    public void Fire()
    {
        if (isReloading || currentArrow == null)
            return;

        var force = spawnArrow.TransformDirection(Vector3.left * firePower);
        currentArrow.Fly(force);
        currentArrow.dmg = Basedmg;

        currentArrow = null;
        Reload();

    }


    private void UpdateSpeed()
    {
        if (enemy == null)
            anim.SetFloat("Speed", 0f, 0.2f, Time.deltaTime);

        if (Vector3.Distance(transform.position, enemy.position) > 1.5f * attackRange && timer <= 0)
        {
            anim.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
            agent.speed = 6;
        }
        else if (Vector3.Distance(transform.position, enemy.position) > 1f * attackRange)
        {
            anim.SetFloat("Speed", 0.5f, 0.2f, Time.deltaTime);
            agent.speed = 4;
        }


    }
}
