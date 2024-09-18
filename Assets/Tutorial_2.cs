using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_2 : MonoBehaviour
{
    [SerializeField] private int number;

    // Start is called before the first frame update
    void Start()
    {
        if(number > 5){
            Debug.Log("Number is greater than 5"); }
        else{
            while(number <= 5){

                // Debug.Log( number >= 1 ?
                //     "You entered "+ number :
                //     "Your number is less than 1 "); 
                // This also works, the switch case format is redundant in this case 
                // The switch case is preferred for the exercise, so this is commented out

                switch (number){
                    case 1:
                        Debug.Log("You entered 1");
                        break;
                    case 2:
                        Debug.Log("You entered 2");
                        break;
                    case 3:
                        Debug.Log("You entered 3");
                        break;
                    case 4:
                        Debug.Log("You entered 4");
                        break;
                    case 5:
                        Debug.Log("You entered 5");
                        break;
                    default:
                        Debug.Log("Your number is less than 1");
                        break;
                }
                break;   
            }
        }

        /*

        Debug.Log("Game started");

        for(int i = 0; i < 5; i++){
            Debug.Log("For Loop iteration: " + i);
        }

        int count;
        while (count < 5){
            Debug.Log("While Loop iteration: " + i);
            count++;
        }

        */
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( Input.GetKeyDown(KeyCode.Space)){

            print("Space was pressed");
        }
    }
}
