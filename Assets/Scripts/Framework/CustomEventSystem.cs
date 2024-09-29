using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Linq;

public class CustomEventSystem : MonoBehaviour
{
    public static void TriggerPrePlayerMove(){
        foreach ( IPretMoveListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IPretMoveListener>()){
            listener.OnPrePlayerMove(Player.current, TurnLoopManager.main.rollOutcome);
        }
    }

    public static void TriggerPosInitMove(bool cards = false){

        if(cards){
            foreach ( IPosInitialtMoveListener listener in CardSystem.main.Unpack() ){
                listener.OnPostPlayerMove(Player.current);
            }

            return; }

        foreach ( IPosInitialtMoveListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IPosInitialtMoveListener>()){
            listener.OnPostPlayerMove(Player.current);
        }
    }

    public static void TriggerPreSabotage(Player player){

        foreach ( ISabotagePlayerListener listener in FindObjectsOfType<MonoBehaviour>().OfType<ISabotagePlayerListener>()){
            listener.OnPlayerSabotage(Player.current, player);
        }
    }
    public static void TriggerNewTurn(){

        foreach ( ITurnSwitchListener listener in FindObjectsOfType<MonoBehaviour>().OfType<ITurnSwitchListener>()){
            listener.OnNextTurn(Player.current);
        }
    }
}

public interface ITurnSwitchListener{
    public void OnNextTurn(Player newPlayer);   
}

public interface ISabotagePlayerListener{
    public void OnPlayerSabotage(Player sabotager, Player sabotagedPlayer);
}

public interface IPosInitialtMoveListener{
    public void OnPostPlayerMove(Player movedPlayer);
}
public interface IPretMoveListener{
    public void OnPrePlayerMove(Player movedPlayer, int initialRollOutcome);
}

public interface IEndOfTurnListener{
    public void OnEndOfTurn();
}
