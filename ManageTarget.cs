using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTarget : MonoBehaviour
{
    public LayerMask whatIsEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform FindTarget(string enemyTag, Transform transform)
    {
        GameObject[] enemies;

        enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float min = 0;
        GameObject target = null;



        if (enemies.Length > 0)
        {

            min = Vector3.Distance(enemies[0].transform.position, transform.position);
            target = enemies[0];

            foreach (GameObject entity in enemies)
            {
                float dist = Vector3.Distance(entity.transform.position, transform.position);


                if (dist < min)
                {
                    min = dist;
                    target = entity;
                }

            }
        }



        if (enemyTag == "ally")
        {

            GameObject player = GameObject.FindWithTag("Player");

            float dist = Vector3.Distance(player.transform.position, transform.position);

            if (target == null)
            {
                min = dist;
                target = player;
            }

            else if (dist < min)
            {
                min = dist;
                target = player;
            }


        }

        if(target != null)
            return target.transform;
        else
            return null;
    }
}
