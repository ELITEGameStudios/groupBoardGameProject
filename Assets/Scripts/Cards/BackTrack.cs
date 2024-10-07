using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrack : Card
{

    public int intensity {get; private set;}
    bool special;

    public BackTrack(Player _host, int _intensity, bool special = false) : base(_host){
        host = _host;

        type = special ? 4 : 3;
        index = special ? 3 : 0;
        this.special = special;

        intensity = _intensity;
        title = special ? "CHAOS": "Back Track";
        // description = "Send a player of your choosing back by " + intensity.ToString() + " Spots!";
        description = special ? 
            $"Send ALL other players back by {intensity} spots!" : 
            $"Send a random player back by {intensity} Spots!";
    }

    public override void Use(){
        Debug.Log("Default Card Use Function | Not Implemented for this card.");
        isActive = true;

        if(!special){// Will sabotage one random player that is not the holder of this card

            int playerIndex = Random.Range(0, Player.players.Count);

            if(Player.players[playerIndex] == host){
                playerIndex = playerIndex == Player.players.Count-1 ? playerIndex-1 : playerIndex+1;
            }
            Player player = Player.players[playerIndex];
            CustomEventSystem.TriggerPreSabotage(player);
            if(!player.isSabotageProtected){// Is the chosen player protected from Back Track?
                // This player is NOT protected. RIP
                TurnLoopManager.main.QueueAdditionalMovement(player, -intensity, player.boardPosition, punch: true);
                PopupUIManager.main.DisplayMessage($"{host.name} SABOTAGED {player.name}! {player.name} loses {intensity} spots!", Color.HSVToRGB(0, 0.7f, 1));
            }
                // The player is protected from this attck
            else{ PopupUIManager.main.DisplayMessage($"{player.name} has BLOCKED {host.name}'s Sabotage!", Color.HSVToRGB(0.8f, 0.7f, 1)); }

        }
        else{ // Will sabotage ALL players except the holder of this card
            foreach (Player player in Player.players){
                if (player == host){continue;}

                CustomEventSystem.TriggerPreSabotage(player);
                if(!player.isSabotageProtected){ // Is the chosen player protected from CHAOS?
                    // This player is NOT protected. RIP
                    TurnLoopManager.main.QueueAdditionalMovement(player, -intensity, player.boardPosition, punch: true);
                    PopupUIManager.main.DisplayMessage($"{host.name} SABOTAGED {player.name}! {player.name} loses {intensity} spots!", Color.HSVToRGB(0, 0.7f, 1));
                }
                // The player is protected from this attck
                else{ PopupUIManager.main.DisplayMessage($"{player.name} has BLOCKED {host.name}'s Sabotage!", Color.HSVToRGB(0.8f, 0.7f, 1)); }
            }
        }

        Retire();
    }
    
    public override void Initialize(){
        Debug.Log("SabotageCard Initialized!");
    }
}
