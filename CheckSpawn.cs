using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSpawn : MonoBehaviour
{
    public Transform CurrentSpawn;
    public Transform Entry;
    public Transform checkpoint;

    void Awake()
    {
        //seteaza spawnul playerului
        if (PlayerPrefs.GetInt("CheckPoint",0) == 1)
            CurrentSpawn = checkpoint;
        else
            CurrentSpawn = Entry;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            CurrentSpawn = checkpoint;
            PlayerPrefs.SetInt("CheckPoint", 1);
            PlayerPrefs.Save();
        }
    }
}
