using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyRoll : Card, IPreMoveListener
{
    int maxRoll;
    public LuckyRoll(Player _host) : base(_host){
        host = _host;

        type = 2;
        index = 0;
        maxRoll = Random.Range(2, 5);
        title = "Lucky Roll";
        description = $"Gain an extra roll if your first is under {maxRoll}.\nOtherwise, go backwards by the amount you rolled";
    }

    public override void Use(){
        Debug.Log("You better be lucky");
        isActive = true;
        //Code to play use animation
    }
    
    public override void Initialize(){
        Debug.Log("lucky draw");
        //Code to play init animation
    }

    public void OnPrePlayerMove(Player movedPlayer, int initialRollOutcome)
    {
        Debug.Log("Method IS being called");

        if(isActive){
            if(initialRollOutcome < maxRoll){
                TurnLoopManager.main.GiveExtraTurn();
                Debug.Log("You get an EXTRA TURN!");
                PopupUIManager.main.DisplayMessage($"{movedPlayer.name} was LUCKY! You gain an EXTRA TURN!", Color.HSVToRGB(0.33f, 0.7f, 1));
            }
            else{
                TurnLoopManager.main.ChangeRollOutcome(-initialRollOutcome);
                PopupUIManager.main.DisplayMessage($"{movedPlayer.name} was UNLUCKY! You lose {initialRollOutcome} spots!", Color.HSVToRGB(0, 0.7f, 1));
            }

            Retire();
        }
    }
}
