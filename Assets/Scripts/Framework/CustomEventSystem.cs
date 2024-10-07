using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System.Linq;

public class CustomEventSystem : MonoBehaviour
{
    public static void TriggerPrePlayerMove(){
        foreach ( IPreMoveListener listener in CardSystem.main.UnpackActiveCards<IPreMoveListener>()){
            listener.OnPrePlayerMove(Player.current, TurnLoopManager.main.rollOutcome);
        }
    }

    public static void TriggerPosInitMove(){
        foreach ( IPosInitialtMoveListener listener in CardSystem.main.UnpackActiveCards<IPosInitialtMoveListener>() ){
            listener.OnPostPlayerMove(Player.current);
        }
    }

    public static void TriggerPreSabotage(Player player){

        foreach ( ISabotagePlayerListener listener in CardSystem.main.UnpackActiveCards<ISabotagePlayerListener>()){
            listener.OnPlayerSabotage(Player.current, player);
        }
    }
    public static void TriggerNewTurn(){

        foreach ( ITurnSwitchListener listener in CardSystem.main.UnpackActiveCards<ITurnSwitchListener>()){
            listener.OnNextTurn(Player.current);
        }

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
public interface IPreMoveListener{
    public void OnPrePlayerMove(Player movedPlayer, int initialRollOutcome);
}