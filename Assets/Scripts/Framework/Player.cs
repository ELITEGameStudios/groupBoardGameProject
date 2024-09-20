using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    // Core instance player data
    public string name {get; private set;}
    public int boardPosition {get; private set;}
    public int ranking {get; private set;}
    // public int cardsUsed {get; private set;}
    // public int movesPerformed {get; private set;} ------------------------- these are kinda redundant for now
    public int laps {get; private set;}
    public static int playerCount {get; private set;} = 0;
    public GameObject gameObject {get; private set;}
    public PlayerObjectUI graphicsHelper {get; private set;}
    public List<Card> currentDeck {get; private set;}
    public bool isSabotageProtected {get; private set;}


    // Lists sorting multiple players in the game
    public static List<Player> players {get; private set;}

    // The currently active player
    public static Player current { get {return players[GameManager.main.currentPlayerIndex];} }



    public Player(string _name){
        name = _name;
        currentDeck = new List<Card>(){null, null, null};
        boardPosition = 0;
        ranking = 1;
        laps = 0;
    }

    public void AddCard(Card newCard){
        for (int i = 0; i < currentDeck.Count; i++){
            if(currentDeck[i] == null){
                currentDeck[i] = newCard;
                currentDeck[i].Initialize();
                return;
            }
        }

        newCard = null;
    }

    public void UseCard(int slot){
        if(slot < currentDeck.Count){
            if(currentDeck[slot] != null){
                Debug.Log("Using this card");
                currentDeck[slot].Use();
                CardUIManager.main.SetCardUI();
                // currentDeck[slot].Retire();
            }
            else{
                Debug.Log("This slot is empty!");
            }

        }
    }

    public void AddLap(){
        laps++;
        if(laps == 3 ){
            // End game here with this player as the winner
        }
    }
    public void SetNewPosition(int newPos, bool newCard = true){
        MapTile newTile = MapManager.main.GetTile(newPos);
        if(newTile != null){
            boardPosition = newPos;
            // Transform player's position
            gameObject.transform.position = newTile.gameObject.transform.position;
            // Do animations, UI, Card logic, and other logic
        }
    }

    public void PickupNewTileCard(){
        MapTile newTile = MapManager.main.GetTile(boardPosition);
        if(newTile != null){
            Card tileCard = newTile.GetNewCard(this);
            if(tileCard != null){ AddCard(tileCard); }
        }
    }

    public void SetGameObject(GameObject _gameObject){
        if(gameObject == null){
            gameObject = _gameObject;
            gameObject.transform.position = MapManager.main.GetTile(boardPosition).gameObject.transform.position;
            graphicsHelper = gameObject.GetComponent<PlayerObjectUI>();
            graphicsHelper.SetName(name);
        }
    }

    public void SabotageProtected(bool _protected){
        isSabotageProtected = _protected;
    }

    public static void InitializePlayers(List<string> playerNames){

        if(players == null){
            players = new List<Player>();
            foreach(string _name in playerNames){
                players.Add(new Player(_name));
            }
            playerCount = players.Count;
        }
        else{
            Debug.Log("Players have already been initialized!");
        }
    }
}
