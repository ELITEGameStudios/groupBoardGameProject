using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTrackBoostCard : Card, IPosInitialtMoveListener
{

    int intensity;
    // Constructor to initialize this specific card type
    public FastTrackBoostCard(Player _host, bool special = false) : base(_host){
        host = _host;

        // Defines the card type, index, title, and description of this specific card
        type = special ? 4 : 0;
        index = 0;
        intensity = special ? Random.Range(10, 15) : Random.Range(2, 6);
        title = special ? "Insta-Travel" : "Fast Track";
        description = $"Advance your character by an additional {intensity.ToString()} spots!";
    }

    // Overridden methods of the base class. Now when other systems call the functions of the base class, these will execute instead
    public override void Use(){
        Debug.Log("Used FastTrack");
        base.Use();
        //Code to play use animation
    }
    
    public override void Initialize(){
        Debug.Log("FastTrack Initialized");
        //Code to play init animation
    }
    
    // Notice that Retire() and Passive()  is not implemented, because I do not need to immplement personalized code for this card on those events

    public void OnPostPlayerMove(Player movedPlayer){
        
        if(isActive){
            TurnLoopManager.main.QueueAdditionalMovement(movedPlayer, intensity, movedPlayer.boardPosition);
            Retire();
        }
    }
}
