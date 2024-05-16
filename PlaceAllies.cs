using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;



public class PlaceAllies : MonoBehaviour
{
    [Header("camera")]
    public Camera cam;
    public Transform camPlace;
    public float manageSpeed;
    public bool GoUp;
    public bool GoDown;
    public MeshRenderer player;
    public Movement playerMovement;
    public GameObject PlayerHands;
    public BoxCollider col;
    public float timer;
    public bool TouchedAtLeastOnce;
    public TMP_Text coins;
    public AudioSource audio;
    public AudioClip PlaceSound;
    public PlayerHealth playerHP;
    public GameObject sword;
    public GameObject bow;
    public GameObject arrow;
    public bool YouWin = true;
    public TMP_Text tipText;
    public bool usedTip1;
    public bool usedTip2;
    public bool usedTip3;
    public bool usedTip4;
    public GameObject endScreen;
    public GameObject theEnd;
    public int[] MoneyPerWave;
    public bool inSettings;

   

    [Header("place Units")]
    public bool CanPlace;
    public int Money;
    public int MoneyForSwordGuy;
    public int MoneyForBowGuy;
    public int MoneyForSwordGuyElite;
    public int MoneyForBowGuyElite;
    public int BuyPrice;
    public Transform selected;
    public Transform SwordGuy;
    public Transform BowGuy;
    public Transform SwordGuyElite;
    public Transform BowGuyElite;
    public Transform allies;
    public Transform enemies;

    [Header("Wave")]

    public bool StartedWave;
    public TMP_Text waveNumber;
    public TMP_Text Objective;
    public int waveIndex;
    public CanvasGroup WaveUi;
    public GameObject[] wave;
    public Transform wavePos;
    public GameObject currentWave;
    public float initialEnemies;
    public CanvasGroup waveText;
    public int MoneyUsedThisWave;
    public bool LoadedWave;
    public AudioClip[] waveClips;
    public AudioSource playerAudio;
    public GameObject aliati;





    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse1) && !usedTip1)
        {
            usedTip1 = true;
            tipText.text = "Fiecare soldat costa bani, apasat pe un soldat de jos pe care ti-l permiti pentru a îl selecta";
         
        }
        if(selected != null && !usedTip2 && usedTip1)
        {
            usedTip2 = true;
            tipText.text = "Apasa oriunde pe pamant pentru a plasa soldatul";

        }
        if(MoneyUsedThisWave != 0 && !usedTip3 && usedTip2 && usedTip1)
        {
            usedTip3 = true;
            tipText.text = "Apasa pe butonul verde pentru a incepe invazia otomana";
        }

        if(StartedWave && !usedTip4 && usedTip3 && usedTip2 && usedTip3)
        {
            usedTip4 = true;
        }






        if(waveIndex > wave.Length)
        {
            if(!YouWin)
            {
                YouWin = true;
                endScreen.SetActive(true);
                StartCoroutine(da());
            }
           

        }


        if (TouchedAtLeastOnce)
        {
            waveText.alpha = Mathf.Lerp(waveText.alpha, 1f, 2 * manageSpeed * Time.deltaTime);
        }

        if (timer > 0)
            timer -= Time.deltaTime;

        if(enemies.childCount == 0 && StartedWave)
        {
            Objective.text = "•Pregateste-te la pod pentru urmatorul atac";
            StartedWave = false;
            col.enabled = true;
            waveIndex++;
            playerHP.health = playerHP.maxHealth;
       
        }
        else if(TouchedAtLeastOnce && StartedWave)
        {
        
            Objective.text = "•Omoara Otomanii: " + (initialEnemies - enemies.childCount) + " / " + initialEnemies;
        }

        waveNumber.text = "Wave: " + waveIndex;
        coins.text = Money.ToString();

        if(GoUp)
        {
            if (waveIndex == 3)
            {
                RenderSettings.fog = true;
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 0.01f, 2 * manageSpeed * Time.deltaTime);
            }
            
            //schimba camera si activeaza functia de a pune aliati   

            player.enabled = false;
            playerMovement.rb.velocity = Vector3.zero;
            playerMovement.enabled = false;
            PlayerHands.SetActive(false);
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camPlace.localPosition, manageSpeed * Time.deltaTime);
            cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, camPlace.localRotation, manageSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 45, 45/manageSpeed * Time.deltaTime);
            cam.enabled = true;
            CanPlace = true;

            WaveUi.alpha =Mathf.Lerp(WaveUi.alpha, 1f, 2 * manageSpeed * Time.deltaTime);

            if (Vector3.Distance(cam.transform.localPosition, camPlace.localPosition) < 0.1f)
            {
                GoUp = false;
             
            }

            sword.SetActive(false);
            bow.SetActive(false);
            arrow.SetActive(false);

        }
        else if(GoDown)
        {
            if(waveIndex != 3)
            {
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 0f, 2 * manageSpeed * Time.deltaTime);
                if (RenderSettings.fogDensity < 0.001)
                    RenderSettings.fog = false;
            }
            else
            {
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 0.025f, 2 * manageSpeed * Time.deltaTime);
            }

            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(0, 0, 0), 2 * manageSpeed * Time.deltaTime);
            cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 0)), 2 * manageSpeed * Time.deltaTime);
            CanPlace = false;
            WaveUi.alpha = Mathf.Lerp(WaveUi.alpha, 0f,2 * manageSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 90, 45/manageSpeed * Time.deltaTime);

            if (Vector3.Distance(cam.transform.localPosition, new Vector3(0, 0, 0)) < 0.1f)
            {
                PlayerHands.SetActive(true);
                GoDown = false;
                cam.enabled = false;
                player.enabled = true;
                playerMovement.enabled = true;
            }

        }

        if(!CanPlace && !inSettings)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        SpawnAtMousePos();
       
    }

    private void SpawnAtMousePos()
    {
        if(Input.GetKey(KeyCode.Mouse0) && CanPlace && selected != null && Money >= BuyPrice && !EventSystem.current.IsPointerOverGameObject() && timer <= 0)
        {
            //gaseste locul in lume unde e cursorul

            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.transform.tag == "terrain")
                {
                    
                    Transform ally;

                    audio.PlayOneShot(PlaceSound, 1f);

                    ally = Instantiate(selected, hit.point, Quaternion.identity, allies);

                    MonoBehaviour[] scripts = ally.GetComponents<MonoBehaviour>();

                    foreach (MonoBehaviour script in scripts)
                    {
                        script.enabled = false;

                    }

                    Money -= BuyPrice;
                    MoneyUsedThisWave += BuyPrice;

                    timer = 0.1f;
                }
            

            }
;        }
    }


    public void SelectBow()
    {
        selected = BowGuy;
        BuyPrice = MoneyForBowGuy;
    }

    public void SelectSword()
    {
        selected = SwordGuy;
        BuyPrice = MoneyForSwordGuy;
    }

    public void SelectBowElite()
    {
        selected = BowGuyElite;
        BuyPrice = MoneyForBowGuyElite;
    }

    public void SelectSwordElite()
    {
        selected = SwordGuyElite;
        BuyPrice = MoneyForSwordGuyElite;
    }

    public void OnTriggerEnter()
    {
        if(!StartedWave)
        {
           
            LoadWave();

            GoUp = true;
            selected = null;
            col.enabled = false;

            if(!TouchedAtLeastOnce)
            {
                TouchedAtLeastOnce = true;
            }
            
        }
    }

    public void StartWave()
    {

        //incepe waveul

        StartedWave = true;
        GoUp = false;
        GoDown = true;

        foreach(Transform child in enemies)
        {
            MonoBehaviour[] scripts = child.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = true;

            }
        }


        if (waveIndex == 4)
        {
            

            Transform[] children = new Transform[currentWave.transform.childCount];

            int i = 0;
            foreach (Transform child in currentWave.transform)
            {
                children[i++] = child;
            }



            foreach (Transform child in children)
            {

                child.SetParent(allies);


            }
        }

        foreach (Transform child in allies)
        {
            MonoBehaviour[] scripts = child.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = true;

            }
        }
    }

    public void LoadWave()
    {
        //incarca waveul

        foreach (Transform child in allies)
        {

            Destroy(child.gameObject);
        }

        playerAudio.clip = waveClips[waveIndex - 1];
        playerAudio.Play();
        currentWave = Instantiate(wave[waveIndex - 1], wavePos.position, Quaternion.identity);
        MoneyUsedThisWave = 0;
        Money = MoneyPerWave[waveIndex - 1];

        Transform[] children = new Transform[currentWave.transform.childCount];

        initialEnemies = currentWave.transform.childCount;

        int i = 0;
        foreach (Transform child in currentWave.transform)
        {
            children[i++] = child;
        }

       
        foreach (Transform child in children)
        {
            
            child.SetParent(enemies);

           
        }

        Destroy(currentWave);

        if (waveIndex == 4)
        {
            currentWave = Instantiate(aliati, wavePos.position, Quaternion.identity);
            currentWave.SetActive(true);
        }
            

        
    }

    public void DeleteAllies()
    {
        Money += MoneyUsedThisWave;
        MoneyUsedThisWave = 0;

        foreach (Transform child in allies)
        {
           
            Destroy(child.gameObject);
        }
    }

    public IEnumerator da()
    {
        yield return new WaitForSeconds(2f);
        playerMovement.enabled = false;
        PlayerHands.SetActive(false);
        yield return new WaitForSeconds(21f);
        theEnd.SetActive(true);
        yield return new WaitForSeconds(3f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }

}
