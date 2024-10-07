using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SabotageDefender : Card, ITurnSwitchListener
{
    int rolls;
    public SabotageDefender(Player _host, bool special = false) : base(_host){
        host = _host;

        rolls = special ? 4 : 2;
        type = special ? 4 : 1;
        index = special ? 1 : 0;
        title = special ? "Guardian" : "Protector";
        description = $"Protects you from a sabotage attack for {rolls} rolls";
    }

    public override void Use(){
        Retire();
    }
    
    public override void Initialize(){
        Debug.Log("Using the shield.");
        host.SabotageProtected(true);
        isActive = true;
        CardSystem.main.activeCards.Add(this);
    }
    
    public override void Retire(){
        Debug.Log($"BYEBYE");
        host.SabotageProtected(false);
        base.Retire();
    }

    public void OnNextTurn(Player newPlayer)
    {
        Debug.Log($"HEY LOOK I ONLY HAVE {rolls} ROLLS LEFT!");
        if (Player.current == host){
            rolls--;
            if(rolls <= 0){ Retire(); }
        }
    }
}