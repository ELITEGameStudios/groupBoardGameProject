using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager main {get; private set;}
    public int currentPlayerIndex {get; private set;}
    public bool twoDice {get; private set;}
    public bool hasStarted {get; private set;}
    [SerializeField] private GameObject playerPrefab; 
    public GameObject GetPlayerPrefab {get {return playerPrefab;} } 
    public List<string> playerNames; 

    // Start is called before the first frame update
    void Awake()
    {
        if(main == null){main = this;}
        else if(main != this){Destroy(this);} 
        currentPlayerIndex = 0;
    }

    void Start(){
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayers(){
        Player.InitializePlayers(playerNames);
        foreach (Player player in Player.players)
        {
            player.SetGameObject(Instantiate(GameManager.main.GetPlayerPrefab, MapManager.main.GetMapHostTransform));
        }
    }

    public void NextTurn(){
        // Goes to the next player in line
        if(currentPlayerIndex < Player.players.Count-1){
            currentPlayerIndex++;
        }
        else{
            currentPlayerIndex = 0;
        }
    }

}
