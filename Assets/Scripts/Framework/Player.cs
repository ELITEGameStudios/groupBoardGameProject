using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    /* --- Welcome to the Player class --- 

        This is where core player data lies. The most core variables used by other scripts is:
            Player.current, which retrieves the player whos turn it is
            Player.players, which is the list of all the players in the game

        All players contain unique data for themselves shown below that isn't static.
        If you want the name of the current player for example, use Player.current.name

        Initializing the player list itself is also handled here, but called from an external script GameManager when setting up the game
    */

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


    // This list contains all the player instances generated in the game
    public static List<Player> players {get; private set;}

    // This is the active player. That is, the player whos turn it is
    // You will see this variable a lot in other scripts the form of Player.current...
    public static Player current { get {return players[GameManager.main.currentPlayerIndex];} }



    public Player(string _name){
        // Generates the initial data for the player, with the given name
        name = _name;
        currentDeck = new List<Card>(){null, null, null};
        boardPosition = 0;
        ranking = 1;
        laps = 0;
    }

    public void AddCard(Card newCard){
        // Adds a card to the deck if one of the cards the player has is null (nothing)
        for (int i = 0; i < currentDeck.Count; i++){
            if(currentDeck[i] == null){
                currentDeck[i] = newCard;
                // Calls the initialize function of the given card
                currentDeck[i].Initialize();
                return;
            }
        }

        newCard = null;
    }

    public void UseCard(int slot){
        // Calls the Use() function of the corresponding card index
        if(slot < currentDeck.Count){
            if(currentDeck[slot] != null){
                Debug.Log("Using this card");
                currentDeck[slot].Use();
                // Updates the UI for the card
                CardUIManager.main.SetCardUI();
            }
            else{
                Debug.Log("This slot is empty!");
            }

        }
    }

    public void EndGame(){
        GameManager.main.ToggleWinScreen();
        players.Clear();
        // laps++;
        // if(laps == 3 ){
        //     // End game here with this player as the winner
        // }
    }

    public void SetNewPosition(int newPos, bool newCard = true){
        // Updates the boardPosition of the player
        MapTile newTile = MapManager.main.GetTile(newPos);
        if(newTile != null){
            boardPosition = newPos;
            // Transform player's position
            gameObject.transform.position = newTile.gameObject.transform.position;
        }
    }

    public void PickupNewTileCard(){
        // Retrieves the map tile that the player is currently situated on, and adds a new card if this tile contains one
        MapTile newTile = MapManager.main.GetTile(boardPosition);
        if(newTile != null){
            Card tileCard = newTile.GetNewCard(this);
            if(tileCard != null){ AddCard(tileCard); }
        }
    }

    public void SetGameObject(GameObject _gameObject){
        // Assigns the player's representative gameObject to this player. 
        // Instantiating the gameObject itself is handled by the GameManager
        if(gameObject == null){
            gameObject = _gameObject;
            gameObject.transform.position = MapManager.main.GetTile(boardPosition).gameObject.transform.position;
            graphicsHelper = gameObject.GetComponent<PlayerObjectUI>();
            gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.main.playerSprites[Random.Range(0, GameManager.main.playerSprites.Length)];
            graphicsHelper.SetName(name);
        }
    }

    public void SabotageProtected(bool _protected){
        isSabotageProtected = _protected;
    }

    public static void InitializePlayers(List<string> playerNames){
        // Generates the list of players in the game with the name given to it
        // Can only be done if the players are not yet generated
        if(players == null){
            players = new List<Player>();
            foreach(string _name in playerNames){
                players.Add(new Player(_name));
            }
            playerCount = players.Count;
        Debug.Log("InitializedPlayers");
        }
        else{
            Debug.Log("Players have already been initialized!");
        }
        Debug.Log("AllegedlyInitializedPlayers");
    }
}
