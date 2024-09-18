using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_1 : MonoBehaviour
{
    private static int age = 10, studentNumber = 100870679;
    private static string fullName = "Noah Simmons";
    private static string[] splitFullName = fullName.Split(' ');
    private static char lastInitial = splitFullName[splitFullName.Length-1][0];
    private static bool over18 = age > 18;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Your Age: is: " + age);
        Debug.Log("Your Student Number: is: " + studentNumber);
        Debug.Log("Your Full Name is: " + fullName);
        Debug.Log("Your Last Initial: is: " + lastInitial);
        
        Debug.Log("You " + (over18 ? "ARE" : "are NOT") +" over 18 ");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
