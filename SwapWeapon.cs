using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapWeapon : MonoBehaviour
{
    public Animator CharacterAnim;
    public GameObject Sword;
    public GameObject Bow;
    public GameObject Arrow;
    public Movement mv;
    public bool ChangingWeapon;
    public Bow bw;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoveAnim();

        if(CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("unequip Sword") && CharacterAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {

            SwordOff();
        }

        if (CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("unequip Bow") && CharacterAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {

            BowOff();
        }

        if(CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("Sword Equip"))
        {
          

            CharacterAnim.SetBool("Sword", true);

            Sword.SetActive(true);

            if(CharacterAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5)
                ChangingWeapon = false;
        }

        if (CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("empty"))
        {
            if (CharacterAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5)
                ChangingWeapon = false;
        }
           

        if (CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("Bow Equip"))
        {
            Bow.SetActive(true);
            Arrow.SetActive(true);

            if (!CharacterAnim.GetBool("Bow"))
                bw.Reload();

            CharacterAnim.SetBool("Bow", true);

           

         ;

            if (CharacterAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5)
                ChangingWeapon = false;


        }


        if (Input.GetKeyDown(KeyCode.Alpha1) && !CharacterAnim.GetBool("Sword") && !ChangingWeapon)
        {
            ChangingWeapon = true;
            CharacterAnim.SetTrigger("ChangeWeapon");
            CharacterAnim.SetTrigger("SwordEquip");

        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !CharacterAnim.GetBool("Bow") && !ChangingWeapon)
        {
            ChangingWeapon = true;
            CharacterAnim.SetTrigger("ChangeWeapon");

            CharacterAnim.SetTrigger("BowEquip");

        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && !CharacterAnim.GetCurrentAnimatorStateInfo(0).IsName("empty") && !ChangingWeapon)
        {
            ChangingWeapon = true;
            CharacterAnim.SetTrigger("ChangeWeapon");
            CharacterAnim.SetTrigger("EmptyHand");
            
        }


    }

    void BowOff()
    {
        CharacterAnim.SetBool("Bow", false);


        Bow.SetActive(false);
        Arrow.SetActive(false);
    }

    void SwordOff()
    {
        CharacterAnim.SetBool("Sword", false);


        Sword.SetActive(false);
    }


    void UpdateMoveAnim()
    {
        if(mv.rb.velocity.magnitude < 0.1f)
        {
            CharacterAnim.SetFloat("Speed", 0f, 0.2f, Time.deltaTime);
        }
        else
        {
            if (mv.moveSpeed == mv.walkSpeed)
                CharacterAnim.SetFloat("Speed", 0.5f, 0.2f, Time.deltaTime);
            else
                CharacterAnim.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);

        }
    }

   
}
