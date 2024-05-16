using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Animator an;
    public float dmg;
    public int noOfClicks = 0;
    public float lastClickedTime = 0;
    float maxComboDelay = 1f;
    public TrailRenderer tr;
    public bool CanHit;
    public bool Blocking;
    public AudioSource audio;
    public AudioClip SwingClip1;
    public AudioClip SwingClip2;
    public bool useSound;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //daca ataci
            if (an.GetCurrentAnimatorStateInfo(0).IsName("Sword Equip"))
            {
                if (an.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
                    GetInput();
            }
            else
                GetInput();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            //daca parezi
            if (an.GetCurrentAnimatorStateInfo(0).IsName("Sword Equip"))
            {
                if (an.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
                {
                    an.SetBool("swordBlock", true);
                    Blocking = true;
                }
                    
            }
            else
            {
                an.SetBool("hit1", false);
                an.SetBool("swordBlock", true);
                Blocking = true;
            }
              
        }

        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            an.SetBool("swordBlock", false);
            Blocking = false;
        }
            




        if (an.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && an.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            an.SetBool("hit1", false);

            //atac1 terminat
        }

        if (an.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && an.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            an.SetBool("hit2", false);
            noOfClicks = 0;

        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
            an.SetBool("hit2", false);
            an.SetBool("hit1", false);
        }


        if ((an.GetCurrentAnimatorStateInfo(0).IsName("hit1") || an.GetCurrentAnimatorStateInfo(0).IsName("hit2")) && an.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && an.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
        {
            tr.emitting = true;
            CanHit = true;
        }
        else
        {
            tr.emitting = false;
            CanHit = false;
        }

    }

    void GetInput()
    {
       
        noOfClicks++;

        if(noOfClicks == 1)
        {
            if (!an.GetBool("hit1") && !an.GetBool("hit2"))
                audio.PlayOneShot(SwingClip1, 1f);

            an.SetBool("hit1", true);
            lastClickedTime = Time.time;

            //atac1
         

        }

        noOfClicks = Mathf.Clamp(noOfClicks, 0, 2);

        if(noOfClicks >= 2 && an.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && an.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            if (!an.GetBool("hit2"))
                audio.PlayOneShot(SwingClip2, 1f);

            an.SetBool("hit1", false);
            an.SetBool("hit2", true);

            //atac2

          
        }

        

       
    }

    void OnTriggerEnter(Collider collider)
    {
         if(!CanHit)
          return;

     
           

        if(collider.tag == "enemy")
        {
            var health = collider.transform.GetComponent<enemyHealth>();
            health.takeDmg(dmg);
        }

    }


    

   
}
