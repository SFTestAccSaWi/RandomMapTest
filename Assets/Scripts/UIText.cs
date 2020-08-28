using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        changeText("TextElement1", "Hi");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool changeText(string gameObjectName, string newText)
    {
        Text txtGameObject = GameObject.Find(gameObjectName).GetComponent<Text>();
        if (txtGameObject != null)
        {
            txtGameObject.text = newText;
            return true;
        }
        return false;
    }
}
