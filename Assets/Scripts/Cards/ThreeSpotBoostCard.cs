using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeSpotBoostCard : Card, IPosInitialtMoveListener
{

    public ThreeSpotBoostCard(Player _host) : base(_host){
        host = _host;

        type = 0;
        index = 0;
        title = "Fast Track";
        description = "Advance your character by an additional three spots!";
    }

    public override void Use(){
        Debug.Log("Used 3SPOTBOOST");
        isActive = true;
        //Code to play use animation
    }
    
    public override void Initialize(){
        Debug.Log("3SPOTBOOST Initialized!");
        //Code to play init animation
    }
    
    public override void Passive(){
        // Debug.Log("This card has no passive function");
        //Code to play passive animation
    }

    public void OnPostInitialPlayerMove(Player movedPlayer){
        
        if(isActive){
            TurnLoopManager.main.MovePlayerPosition(movedPlayer.boardPosition+3, TurnLoopManager.main.AtMiddle);
            Retire();
        }
    }

    // public void Retire(){
    //     //Code to play retire animation
    // }
}
