using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowForTut : MonoBehaviour
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
        // if(flying && transform.localRotation.z < 55)
        // {
        //     transform.Rotate((transform.rotation * Vector3.down) * torque * Time.deltaTime, Space.Self);
        // }


        if (t < maxTime && didHit)
            t += Time.deltaTime;

        else if (t >= maxTime)
            Destroy(gameObject);

    }

    public void Fly(Vector3 force)
    {
        rb.isKinematic = false;
        transform.SetParent(null);
        rb.AddForce(force, ForceMode.Impulse);

        force = force.normalized;
        ArrowDirection = force;

        Vector3 torqueAxis = Vector3.Cross(force, Vector3.up).normalized;
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


        tr.emitting = false;
      
        if (collider.tag == "Player")
        {
            var health = collider.transform.parent.GetComponent<PlayerHealth>();

            if (sk.Blocking)
            {
                Debug.Log(Vector3.Angle(ArrowDirection,cam.forward));

                if (Vector3.Angle(ArrowDirection, cam.forward) < 150)
                {


                    health.takeDmg(dmg);
                }
                else
                {
                    sp.lovituriBlocate++;
                }
            }
            else
            {

                health.takeDmg(dmg);
            }


            Destroy(gameObject);
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        transform.SetParent(collider.transform);

    }

}