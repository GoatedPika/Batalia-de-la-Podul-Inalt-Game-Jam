using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Transform GameOverScreen;
    public Image Healthbar;
    public PlaceAllies pl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthBarFiller();
        ColorChanger();
    }

    public void takeDmg(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            pl.inSettings = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            GameOverScreen.gameObject.SetActive(true);
           
        }
          
    }

    public void HealthBarFiller()
    {
        Healthbar.fillAmount = Mathf.Lerp(Healthbar.fillAmount, health / maxHealth, 3 * Time.deltaTime);
    }

    public void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, health / maxHealth);

        Healthbar.color = healthColor;
      
    }

    public void DeathAnim()
    {

    }
}
