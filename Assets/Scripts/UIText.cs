using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    Text instruction;
    // Start is called before the first frame update
    void Start()
    {
        instruction = GameObject.Find("TextElement1").GetComponent<Text>();
        instruction.text = "Hi"; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
