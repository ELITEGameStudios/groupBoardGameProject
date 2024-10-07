using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStealer : Card, ITurnSwitchListener
{
    int intensity, step;

    public BaseStealer(Player _host, bool special = false) : base(_host){
        host = _host;

        type = special ? 4 : 1;
        index = 1;
        step = special ? 2 : 1;
        intensity = special ? Random.Range(0, 2) : Random.Range(0, 3);
        title = special ? "Thief" : "Base Stealer";
        description = $"{(intensity+1) / 5f * 100} percent chance of move { (special ? "three tiles" : "one tile")} after every turn";
    }

    public override void Use(){
        Debug.Log("Retiring passive base stealer");
        Retire();
    }
    
    public override void Initialize(){
        Debug.Log("Passive base stealer Initialized! and active");
        isActive = true;
        CardSystem.main.activeCards.Add(this);
    }

    public void OnNextTurn(Player newPlayer)
    {
        if(Random.Range(0, 5) <= 1){
            TurnLoopManager.main.QueueAdditionalMovement(host, step, host.boardPosition);
        }
    }
}