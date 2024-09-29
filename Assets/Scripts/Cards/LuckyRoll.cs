using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyRoll : Card, IPretMoveListener
{
    int maxRoll;
    public LuckyRoll(Player _host) : base(_host){
        host = _host;

        type = 0;
        index = 0;
        maxRoll = Random.Range(2, 5);
        title = "Lucky Roll";
        description = "Gain an extra roll if your first is under 4.\nOtherwise, go backwards by the amount you rolled";
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


    public void OnPrePlayerMove(Player movedPlayer, int initialRollOutcome)
    {
        if(isActive){
            if(initialRollOutcome < maxRoll){
                TurnLoopManager.main.GiveExtraTurn();
                Debug.Log("You get an EXTRA TURN!");
            }
            else{
                TurnLoopManager.main.ChangeRollOutcome(-initialRollOutcome);
                Debug.Log("You are BANISHED");
            }

            Retire();
        }
    }
}
