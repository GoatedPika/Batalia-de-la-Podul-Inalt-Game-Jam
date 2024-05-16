using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ringWhenChangeQuest : MonoBehaviour
{
    public AudioSource audio;
    public string curText;
    public string lastText;
    public TextMeshProUGUI text;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curText = text.text;

        if (lastText != curText)
            audio.Play();

       
    }
    void LateUpdate()
    {
        lastText = curText;
    }
}
