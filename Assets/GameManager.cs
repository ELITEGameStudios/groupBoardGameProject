using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    
    public static GameManager main {get; private set;}
    public int currentPlayerIndex {get; private set;}
    public bool twoDice {get; private set;}
    public bool hasStarted {get; private set;}
    [SerializeField] private GameObject playerPrefab; 
    public GameObject GetPlayerPrefab {get {return playerPrefab;} } 
    public List<string> playerNames; 
    [SerializeField] private TMP_Text playerTurnDisplay;




    /* --- Welcome to the GameManager! ---
        Here I (Noah) will describe some general things when using this game's framework.

        Players and their data are represented in instances of the Player script. Find this script in Scripts/Framework to learn more
        The main loop is held in TurnLoopManager located in Scripts/GameplayLoop
        Tiles are represented as instances of the MapTile class found in Scripts/Framework
        Map generation and all the tiles in the map are stored in a list found in the MapManager class in Scripts/Framework
        The base Card script which all card scripts inherit from is located in Scripts/Framework, but all card types are found in Scripts/Cards
        The Card System which handles some card related functionality is found in Scripts/Framework
        Scripts handling most of the changing UI elements are found under Scripts/UI
        The custom event system sccript is found under Scripts/Framework, and defines interfaces which any class can inherit, to implement functionality on custom events in the game
            Cards specifically may use this quite a bit

        The GameManager itself doesn't handle much (in an ideal world, I probably shouldve implemented everything in the TurnLoopManager into this instead)
        The GameManager DOES start the game, handles some initialization functions, and also stores the current player index controlling who's turn it is


        Please note the systems you see can be simplified or expanded on overtime. Currently, the card system might be simplified 
    */

    // Start is called before the first frame update
    void Awake()
    {
        // You will see this in Awake often in many scripts. This creates whats called a "Singleton" variable
        // It essentially ensures that only ONE instance of this script is running, and that it is easily accesible by other scripts via a static variables
        if(main == null){main = this;}
        else if(main != this){Destroy(this);}

        // Sets the initial player index
        currentPlayerIndex = 0;
    }

    void Start(){
        // This is here for the time that there isnt a menu before starting the actual game
        // This should be removed once a main menu has been implemented
        BeginGame();
    }

    public void BeginGame(){
        playerNames = new List<string>();
        playerNames.Add("1690");
        playerNames.Add("2056");
        playerNames.Add("254");
        playerNames.Add("1323");
        playerNames.Add("4414");
        playerNames.Add("5460");
        playerNames.Add("3357");
        playerNames.Add("118");
        playerNames.Add("1241");
        playerNames.Add("5406");
        playerNames.Add("5409");
        playerNames.Add("5596");
        SetPlayers();

        currentPlayerIndex = 0;
        hasStarted = true;
    } 

    public void SetPlayers(){
        // Tells the player class to generate the players with these given names
        Player.InitializePlayers(playerNames);
        foreach (Player player in Player.players)
        {
            // Creates the gameObjects which represent the players
            player.SetGameObject(Instantiate(GameManager.main.GetPlayerPrefab, MapManager.main.GetMapHostTransform));
        }
    }

    public void NextTurn(){
        // Goes to the next player in line
        if(currentPlayerIndex < Player.players.Count-1){
            currentPlayerIndex++;
        }
        else{ currentPlayerIndex = 0; }
        SetTurnUI();
    }

    void SetTurnUI(){
        // A simple piece of text denoting who's turn it is
        playerTurnDisplay.text = Player.current.name + "'s Turn!";
    }

}
