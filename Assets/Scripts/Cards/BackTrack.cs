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
        // description = "Send a player of your choosing back by " + intensity.ToString() + " Spots!";
        description = "Send a random player back by " + intensity.ToString() + " Spots!";
    }

    public override void Use(){
        Debug.Log("Default Card Use Function | Not Implemented for this card.");
        isActive = true;

        int playerIndex = Random.Range(0, Player.players.Count);

        if(Player.players[playerIndex] == host){
            playerIndex = playerIndex == Player.players.Count-1 ? playerIndex-1 : playerIndex+1;
        }
        Player player = Player.players[playerIndex];
        CustomEventSystem.TriggerPreSabotage(player);
        if(!player.isSabotageProtected){
            TurnLoopManager.main.MovePlayerPosition(player.boardPosition - intensity, player, TurnLoopManager.main.PlayerAtMiddle(player));
        }

        Retire();
        //Code to play use animation
    }
    
    public override void Initialize(){
        Debug.Log("SabotageCard Initialized!");
        //Code to play init animation
    }
}
