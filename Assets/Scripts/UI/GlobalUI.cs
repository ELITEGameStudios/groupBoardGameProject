using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUI : MonoBehaviour
{
    [SerializeField] private Text rollOutcomeText;
    public Text GetRollOutcomeText(){return rollOutcomeText;}

    public static GlobalUI main;


    void Awake(){
        if(main == null){main = this;}
        else if(main != this){Destroy(this);}
    }

}
