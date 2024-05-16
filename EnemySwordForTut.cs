using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordForTut : MonoBehaviour
{
    public EnemySwordAi ai;
    public Transform owner;
    public SwordAttack sk;
    public Transform cam;
    public SpawnEnemyForTutorial sp;


    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        var SwordRef = player.transform.parent.GetComponent<PlayerSwordReference>();
        cam = Camera.main.transform;
        sk = SwordRef.sk;

        GameObject tut = GameObject.Find("Tutorial");
        sp = tut.transform.GetChild(0).GetComponent<SpawnEnemyForTutorial>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter(Collider collider)
    {
        if (!ai.CanHit)
            return;


        if ((ai.whatIsEnemy.value & 1 << collider.gameObject.layer) > 0)
        {


            if (collider.tag == "Player")
            {
                Vector3 direction = (owner.position - collider.transform.position).normalized;

                var health = collider.transform.parent.GetComponent<PlayerHealth>();

                if (sk.Blocking)
                {
                    

                    if (Vector3.Angle(direction, cam.forward) > 45)
                    {
                    
                     health.takeDmg(ai.dmg);
                    }
                   
                    else
                    {
                        sp.lovituriBlocate++;
                    }
                }
                else
                {

                    health.takeDmg(ai.dmg);
                }
            }

            else
            {


                var health = collider.transform.GetComponent<enemyHealth>();

                health.takeDmg(ai.dmg);

            }




        }

    }
}
