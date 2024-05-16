using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public float dmg;
    public float torque;
    public Rigidbody rb;
    public bool didHit;
    public float t;
    public float maxTime;
    public bool flying;
    public TrailRenderer tr;

    public SwordAttack sk;
    public Transform cam;
    public Vector3 ArrowDirection;
    public ManageTarget eb;


    // Start is called before the first frame update
    void Start()
    {
        //verifica daca e aliat sau inamic cu playerul ca sa primeasca layer

        GameObject enemies;
        if (gameObject.name == "enemyArrow(Clone)")
            enemies = GameObject.Find("Enemies");
        else
            enemies = GameObject.Find("Allies");
        eb = enemies.transform.GetComponent<ManageTarget>();
        GameObject player = GameObject.FindWithTag("Player");
        var SwordRef = player.transform.parent.GetComponent<PlayerSwordReference>();
        cam = Camera.main.transform;
        sk = SwordRef.sk;
    }

    // Update is called once per frame
    void Update()
    {
      //dupa timp sa dispara sageata

        if (t < maxTime && didHit)
            t += Time.deltaTime;

        else if (t >= maxTime)
            Destroy(gameObject);

    }

    public void Fly(Vector3 force)
    {
        //
        rb.isKinematic = false;
        transform.SetParent(null);
        rb.AddForce(force, ForceMode.Impulse);

        force = force.normalized;
        ArrowDirection = force;

        Vector3 torqueAxis = Vector3.Cross(force,Vector3.up).normalized;
        rb.AddTorque(-torqueAxis * torque, ForceMode.Acceleration);

       
        gameObject.layer = LayerMask.NameToLayer("arrow");
        flying = true;
        tr.emitting = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (didHit || !flying || collider.tag == "cantTouch" || collider.tag == "PlayerSword" || collider.tag == "sabie")
            return;

        flying = false;
        didHit = true;

       //odata ce atinge ceva verifica daca este player sau nu

        tr.emitting = false;

        if(collider.tag != "Player")
        {
            if ((eb.whatIsEnemy.value & 1 << collider.gameObject.layer) > 0)
            {
                
                var health = collider.GetComponent<enemyHealth>();
                health.takeDmg(dmg);

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
                transform.SetParent(collider.transform);
            }
        }

        else
        {
            if(gameObject.name != "AllyArrow(Clone)")
            {
                var health = collider.transform.parent.GetComponent<PlayerHealth>();

                if (sk.Blocking)
                {


                    if (Vector3.Angle(ArrowDirection, cam.forward) < 150)
                    {

                        health.takeDmg(dmg);
                    }
                }
                else
                {

                    health.takeDmg(dmg);
                }


                Destroy(gameObject);
            }
         
        }

       

    }

}
