using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupUIManager : MonoBehaviour
{
    [SerializeField] private GameObject popupObject;
    [SerializeField] private Text popupText;
    [SerializeField] private bool active;
    [SerializeField] private float timer;
    public static PopupUIManager main {get; private set;}
    
    void Awake(){
        if(main == null){main = this;}
        else if(main != this){Destroy(this);}
    }

    // Start is called before the first frame update
    public void DisplayMessage(string message, Color color, int time = 4)
    {
        timer = time;
        popupObject.SetActive(true);
        popupObject.GetComponent<Image>().color = color;
        popupText.text = message;
        active = true;
    }


    // Update is called once per frame
    void Update()
    {
        if(timer > 0){
            timer -= Time.deltaTime;
        }
        else if(active){
            active = false;
            popupObject.SetActive(false);
        }
    }
}
