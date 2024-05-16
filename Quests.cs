using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quests : MonoBehaviour
{
    public TMP_Text obiectiv;
    public bool pod;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider col)
    {
        if(!pod && col.tag == "Player")
        {
            obiectiv.text = "Mergi la dreapta pe pod";
            Destroy(gameObject);
        }
    }
}
