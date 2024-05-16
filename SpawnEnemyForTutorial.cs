using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnEnemyForTutorial : MonoBehaviour
{
    public Transform enemy;
    public Transform enemy1;
    public Transform enemy2;
    public Transform SpawnLocation;
    public Transform SpawnLocation2;
    public Transform enemiesParent;
    public bool Spawned1;
    public bool Spawned2;
    public bool Spawned3;
    public CanvasGroup tip;
    public TMP_Text obiectiv;
    public TMP_Text text;
    public bool usedTip;
    public GameObject barrier;
    public int lovituriBlocate;
    public enemyHealth eh;
    public bool obj4;
    public AudioSource audio;
    public AudioClip clip;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!usedTip)
        {
            tip.alpha = Mathf.Lerp(tip.alpha, 1, Time.deltaTime);
        }
        else
        {
            tip.alpha = Mathf.Lerp(tip.alpha, 0, Time.deltaTime);
        }



        if(Input.GetKeyDown(KeyCode.Alpha1) && text.text == "Apasa 1 pentru sabie" && !usedTip)
        {
          
            usedTip = true;

        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && text.text == "Apasa pe 2 pentru arc" && !usedTip)
        {
            obj4 = true;
            usedTip = true;
            StartCoroutine(wait());

        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && text.text == "Apasa 3 pentru a lasa arma din mana" && !usedTip)
        {
            
            usedTip = true;
          
            

        }



        if (!usedTip)
        {
            if (Input.GetKey(KeyCode.Mouse1) && text.text == "Tine apasat click dreapta pentru a trage sfoara")
            {
                text.text = "Apasa click stanga cat timp tragi sfoara pentru a trage cu arcul";

                

            }
            else if (text.text == "Apasa click stanga cat timp tragi sfoara pentru a trage cu arcul")
            {
                if(Input.GetKeyUp(KeyCode.Mouse1))
                    text.text = "Tine apasat click dreapta pentru a trage sfoara";

                else if(Input.GetKeyDown(KeyCode.Mouse0))
                        usedTip = true;
            }
        }

 
        if (Input.GetKeyDown(KeyCode.Mouse1) && text.text == "Tine apasat click dreapta pentru a bloca" && !usedTip)
        {
         
            usedTip = true;

        }

        if(enemy == null && Spawned1 && !Spawned2)
        {
            enemy = Instantiate(enemy1, SpawnLocation.position, Quaternion.Euler(0, 90, 0));
            enemy.parent = enemiesParent;
            Spawned2 = true;
            text.text = "Tine apasat click dreapta pentru a bloca";
            usedTip = false;
            lovituriBlocate = 0;
            
          
        }

        if(Spawned2 && enemy != null && !Spawned3)
        {
            if(lovituriBlocate >= 2)
            {
                obiectiv.text = "•Omoara otomanul";
                
            }
            else
            {
                obiectiv.text = "•Blocheaza 2 lovituri " + lovituriBlocate + " / 2";
                eh = enemy.GetComponent<enemyHealth>();
                eh.Health = 100;
            }
             
        }

        if(Spawned2 && enemy == null && !Spawned3)
        {
            enemy = Instantiate(enemy2, SpawnLocation2.position, Quaternion.Euler(0, 90, 0));
            enemy.parent = enemiesParent;
            Spawned3 = true;
            lovituriBlocate = 0;
        }

        if (Spawned3 && enemy != null && !obj4)
        {
            if (lovituriBlocate >= 1)
            {
                
                    obiectiv.text = "•Omoara otomanul";
                    text.text = "Apasa pe 2 pentru arc";
                    usedTip = false;
                
            }
            else
            {
                obiectiv.text = "•Blocheaza 1 sageata " + lovituriBlocate + " / 1";
                eh = enemy.GetComponent<enemyHealth>();
                eh.Health = 100;
            }
        }


        if (Spawned3 && enemy == null && barrier != null)
        {
            Destroy(barrier);
            obiectiv.text = "•Urmareste calea";
            text.text = "Apasa 3 pentru a lasa arma din mana";
            usedTip = false;
        }

        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            if(!Spawned1)
            {
                enemy = Instantiate(enemy1, SpawnLocation.position, Quaternion.Euler(0, 90, 0));
                enemy.parent = enemiesParent;
                Spawned1 = true;
                text.text = "Apasa 1 pentru sabie";
                usedTip = false;
                obiectiv.text = "•Omoara primul soldat otoman";
                audio.clip = clip;
                audio.Play();

            }

            
        }
    }

    public IEnumerator wait()
    {
        yield return new WaitForSeconds(0.25f);
        text.text = "Tine apasat click dreapta pentru a trage sfoara";
        usedTip = false;
    }
}
