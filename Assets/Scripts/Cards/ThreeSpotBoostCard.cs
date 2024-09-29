using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeSpotBoostCard : Card, IPosInitialtMoveListener
{
    // Constructor to initialize this specific card type
    public ThreeSpotBoostCard(Player _host) : base(_host){
        host = _host;

        // Defines the card type, index, title, and description of this specific card
        type = 0;
        index = 0;
        title = "Fast Track";
        description = "Advance your character by an additional three spots!";
    }

    // Overridden methods of the base class. Now when other systems call the functions of the base class, these will execute instead
    public override void Use(){
        Debug.Log("Used 3SPOTBOOST");
        isActive = true;
        //Code to play use animation
    }
    
    public override void Initialize(){
        Debug.Log("3SPOTBOOST Initialized!");
        //Code to play init animation
    }
    
    // Notice that Retire() and Passive()  is not implemented, because I do not need to immplement personalized code for this card on those events

    public void OnPostPlayerMove(Player movedPlayer){
        
        if(isActive){
            TurnLoopManager.main.MovePlayerPosition(movedPlayer.boardPosition+3, movedPlayer,TurnLoopManager.main.AtMiddle);
            Retire();
        }
    }
}
