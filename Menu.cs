using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    
    public void PlayGame()
    {
        StartCoroutine(da());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator da()
    {
        yield return new WaitForSeconds(28f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
        


}
