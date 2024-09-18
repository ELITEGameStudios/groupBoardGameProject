using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrack : Card
{

    public int intensity {get; private set;}

    public BackTrack(Player _host, int _intensity) : base(_host){
        host = _host;

        type = 0;
        index = 0;

        intensity = _intensity;
        title = "Back Track";
        description = "Send a player of your choosing back by " + intensity.ToString() + " Spots!";
    }

    public override void Use(){
        Debug.Log("Default Card Use Function | Not Implemented for this card.");
        isActive = true;
        //Code to play use animation
    }
    
    public override void Initialize(){
        Debug.Log("ThreeSpotBoostCard Initialized!");
        //Code to play init animation
    }
    
    public override void Passive(){
        Debug.Log("This card has no passive function");
        //Code to play passive animation
    }
    
    public override void Retire(){
        //Code to play retire animation
    }

    // public void Retire(){
    //     //Code to play retire animation
    // }
}
