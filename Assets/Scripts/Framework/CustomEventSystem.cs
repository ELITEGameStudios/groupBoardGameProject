using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Linq;

public class CustomEventSystem : MonoBehaviour
{
    public static void TriggerPosInitMove(bool cards = false){

        if(cards){
            foreach ( IPosInitialtMoveListener listener in CardSystem.main.Unpack() ){
                listener.OnPostInitialPlayerMove(Player.current);
            }

            return; }

        foreach ( IPosInitialtMoveListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IPosInitialtMoveListener>()){
            listener.OnPostInitialPlayerMove(Player.current);
        }
    }
}

public interface IPlayerSwitchListener{
    public void OnPlayerSabotage(Player oldPlayer, Player newPlayer);   
}

public interface ISabotagePlayerListener{
    public void OnPlayerSabotage(Player sabotager, Player sabotagedPlayer);
}

public interface IPosInitialtMoveListener{
    public void OnPostInitialPlayerMove(Player movedPlayer);
}

public interface IEndOfTurnListener{
    public void OnEndOfTurn();
}
