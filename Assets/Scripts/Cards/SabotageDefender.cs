using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SabotageDefender : Card
{

    public SabotageDefender(Player _host) : base(_host){
        host = _host;

        type = 0;
        index = 0;
        title = "Shield";
        description = "Protects you from a sabotage attack until your next roll";
    }

    public override void Use(){
        Debug.Log("Default Card Use Function | Not Implemented for this card.");
        host.SabotageProtected(true);
        isActive = true;
        //Code to play use animation
    }
    
    public override void Initialize(){
        Debug.Log("Sabotage Defender Initialized!");

        //Code to play init animation
    }
    
    public override void Passive(){
        Debug.Log("This card has no passive function");
        //Code to play passive animation
    }
    
    public override void Retire(){
        host.SabotageProtected(false);
        isRetired = true;
        //Code to play retire animation
    }


}