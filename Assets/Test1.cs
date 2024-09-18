using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test1 : MonoBehaviour
{

    // Declare the variables, have it visible from the inspector
    [SerializeField] private int health, armor;

    void Start(){
        if(health > 50){
            Debug.Log("Safe");
        }
        else{
            if(armor > 30){
                Debug.Log("You are protected by armor, but health is low");
            }
            else{
                Debug.Log("Critical condition, both health and armor is low");
            }
        }
    }
}
