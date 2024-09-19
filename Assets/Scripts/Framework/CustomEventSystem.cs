using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEventSystem
{

    public static void TriggerPosInitMove(){

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
