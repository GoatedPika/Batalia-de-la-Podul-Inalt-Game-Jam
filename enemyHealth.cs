using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHealth : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    public Image healthBar;
    public Transform bar;
    private Camera cam;
    


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();

        Vector3 dir = cam.transform.position - bar.position;
        bar.rotation = Quaternion.LookRotation(dir);
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, Health / MaxHealth, 5 * Time.deltaTime);
    }

    public void takeDmg(float dmg)
    {
        Health -= dmg;

        if (Health <= 0)
        {
          

            Destroy(gameObject);
        }
           
    }
}
