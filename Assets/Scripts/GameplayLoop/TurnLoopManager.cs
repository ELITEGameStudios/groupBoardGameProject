using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLoopManager : MonoBehaviour
{
    bool requiresMiddleCondition;
    [SerializeField] private bool twoDice;
    [SerializeField] private bool doNextTurn = true, middleCondition = false, requiresRoll = true;
    [SerializeField] private int projectedPosition, spotsToMiddleConnector, rollOutcome;
    [SerializeField] private GameObject middleChoiceUI;
    public int currentTurnState = 0; // 0 is pre-roll, 1 is during initial move, 2 is post first move
    public static TurnLoopManager main { get; private set;}

    void Awake() {
        if(main == null){ main = this; }
        else if(main != this ){ Destroy(this);}

        StartCoroutine("MainLoopNumerator"); // Starts the main game loop
    }

    void PromptMiddleCondition(){
        // UI Stuff
        requiresMiddleCondition = true;
        middleChoiceUI.SetActive(true);

    }

    void CheckPostMoveCards(){
        if(CardSystem.main.activeCards.Count > 0){
            CardSystem.main.TriggerPostMoveFunction();
        }
    }

    // public void PassMiddleCondition(bool condition){
    //     // Disable UI
    //     if(requiresMiddleCondition){
    //         MovePlayerPosition(projectedPosition, condition);
    //         currentTurnState = 2;

    //         requiresMiddleCondition = false;
    //         middleChoiceUI.SetActive(false);
    //     }
    // }
    
    public void PassMiddleCondition(bool condition){
        // Disable UI
        if(requiresMiddleCondition){
            requiresMiddleCondition = false;
            middleCondition = condition;

            middleChoiceUI.SetActive(false);
        }
    }



    public void DoRollSystem(){
        requiresRoll = false;
        // Roll();
        // CustomEventSystem.TriggerPosInitMove();
        
        // if(doNextTurn){ GameManager.main.NextTurn(); }
    }

    public bool AtMiddle {
        get {
            return Player.current.boardPosition >= MapManager.main.outerTilesLength; // If the players board position is over the outer tiles position BEFORE the roll, then the player is on a middle tile
        }
    }



    public int Roll(){
        currentTurnState = 1;
        // Generate the outcome of the dice roll
        rollOutcome = Random.Range(1, twoDice ? 13 : 7); // Generates a randomized outcome with ranges dependant on if the user has a second dice or not
        
        // rollOutcome = 1;
        
        projectedPosition = Player.current.boardPosition + rollOutcome; // The position the player should be after this move has been completed
        // DoUiFunction();
        
        if(AtMiddle){
            // MovePlayerPosition(projectedPosition, true, rollOutcome); // Feeds the projected pos to the movePlayer function
            currentTurnState = 2;
            return rollOutcome;
        }

        if(TestForMiddleCondition(Player.current.boardPosition, projectedPosition)){
            spotsToMiddleConnector = MapManager.main.GetSpotsPastMiddleConnector(projectedPosition);
            PromptMiddleCondition();
            currentTurnState = 2;
            
            return rollOutcome;
        }

        // MovePlayerPosition(projectedPosition, false); // Feeds the projected pos to the movePlayer function
        currentTurnState = 2;
        return rollOutcome;
    }

    bool TestForMiddleCondition(int prePosition, int postPosition){
        MapTile enterTile = MapManager.main.GetMiddleTile();
        if(enterTile != null){
            if(enterTile.index >= prePosition && enterTile.index < postPosition){
                
                return true;
            }
        }
        

        return false;
    }
    public void MovePlayerPosition(int newPosition, bool middle = false, int roll = -1){
        // Moving forward, middle tile detection must still be added with the option of travelling through the mid lane
        if(middle){
            if(newPosition >= MapManager.main.totalTiles){
                // If this condition is met, the player has passed the mid section and is now trying to access a position thats not on the board
                // We correct this by setting the position to the difference between the max outer tile index and the projected position
                int overlap = newPosition - MapManager.main.totalTiles;
                newPosition = MapManager.main.GetMiddleTile(false).index + overlap;

                Debug.Log("MidPos: "+MapManager.main.GetMiddleTile(false).index.ToString());
                Debug.Log("Overlap: "+overlap.ToString() + "\nTotal t: " +MapManager.main.totalTiles.ToString() + "\nNewPos: " + newPosition.ToString());
                middle = false;
                
            }
            else if(!AtMiddle) { // This means the player is currently on the normal board, and transitioning to the middle
                int overlapSpots = MapManager.main.GetSpotsPastMiddleConnector(newPosition);
                newPosition = (MapManager.main.outerTilesLength-1) + overlapSpots;
            }
            

        }

        if(newPosition >= MapManager.main.outerTilesLength && !middle){
            // If this condition is met, the player has completed a loop and is now trying to access a position thats not on the board
            // We correct this by setting the position to the difference between the max outer tile index and the projected position
            newPosition -= MapManager.main.outerTilesLength;
            Player.current.AddLap(); // Adding a lap to the player's data
        }
        Debug.Log("Moved the player to " + newPosition.ToString());
        Player.current.SetNewPosition(newPosition);
        currentTurnState = 2;
        // CheckCards();

    }



    IEnumerator MainLoopNumerator(){
        
        while(true){
            // NormalTurnLoopPhase
            while(requiresRoll) {yield return null; } // Halts execution until the player wants to roll

            int rollOutcome = Roll(); // Rolls and stores outcome

            if(requiresMiddleCondition){ // Checks if the middle condition is required
                while (requiresMiddleCondition){ yield return null; } // Stops execution until the player decides which path to choose
                // When PassMiddleConditionNew is called, it will set requiresMiddleCondition to false and set the middlePosition variable accordingly
                
                MovePlayerPosition(projectedPosition, middleCondition, rollOutcome); // Moves the player through the chosen path 
            }
            else{
                MovePlayerPosition(projectedPosition, AtMiddle, rollOutcome); // A standard roll and move if the ccondition is not required
            }

            // Logic after the main roll
            CheckPostMoveCards();

            //Move Player To
            // if(doNextTurn){ // A method will be made so that the card system can halt the next turn if a card deems so
            //     GameManager.main.NextTurn(); 
            // }
            
            Player.current.PickupNewTileCard();
            GameManager.main.NextTurn(); 
            CardUIManager.main.SetCardUI();

            Debug.Log("Done");

            requiresRoll = true; // Ensures that a player will need to provide input before this loop repeats
        }


    }
}
