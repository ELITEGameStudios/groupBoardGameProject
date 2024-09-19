using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLoopManager : MonoBehaviour
{
    bool requiresMiddleCondition;
    [SerializeField] private bool twoDice;
    [SerializeField] private bool doNextTurn = false;
    [SerializeField] private int projectedPosition, spotsToMiddleConnector, rollOutcome;
    [SerializeField] private GameObject middleChoiceUI;
    public int currentTurnState = 0; // 0 is pre-roll, 1 is during initial move, 2 is post first move

    void PromptMiddleCondition(){
        // UI Stuff
        requiresMiddleCondition = true;
        middleChoiceUI.SetActive(true);

    }

    void CheckCards(){
        if(CardSystem.main.activeCards.Count > 0){
            // CardSystem.main.TriggerPostMoveFunction(true);
        }
    }

    public void PassMiddleCondition(bool condition){
        // Disable UI
        if(requiresMiddleCondition){
            MovePlayerPosition(projectedPosition, condition);
            currentTurnState = 2;

            requiresMiddleCondition = false;
            middleChoiceUI.SetActive(false);
        }
    }

    public void DoRollSystem(){
        Roll();
        CustomEventSystem.TriggerPosInitMove();
        
        // if(doNextTurn){ GameManager.main.NextTurn(); }
        GameManager.main.NextTurn(); 
        CardUIManager.main.SetCardUI();
    }



    public void Roll(){
        currentTurnState = 1;
        // Generate the outcome of the dice roll
        bool AtMiddle = Player.current.boardPosition >= MapManager.main.outerTilesLength; // If the players board position is over the outer tiles position BEFORE the roll, then the player is on a middle tile
        rollOutcome = Random.Range(1, twoDice ? 13 : 7); // Generates a randomized outcome with ranges dependant on if the user has a second dice or not
        
        // rollOutcome = 1;
        
        projectedPosition = Player.current.boardPosition + rollOutcome; // The position the player should be after this move has been completed
        // DoUiFunction();
        
        if(AtMiddle){
            MovePlayerPosition(projectedPosition, true, rollOutcome); // Feeds the projected pos to the movePlayer function
            currentTurnState = 2;
            return;
        }

        if(TestForMiddleCondition(Player.current.boardPosition, projectedPosition)){
            spotsToMiddleConnector = MapManager.main.GetSpotsPastMiddleConnector(projectedPosition);
            PromptMiddleCondition();
            currentTurnState = 2;
            
            return;
        }

        MovePlayerPosition(projectedPosition, false); // Feeds the projected pos to the movePlayer function
        currentTurnState = 2;
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
            else if(Player.current.boardPosition < MapManager.main.outerTilesLength) { // This means the player is currently on the normal board, and transitioning to the middle
                int overlapSpots = MapManager.main.GetSpotsPastMiddleConnector(newPosition);
                Debug.Log("Outertileslength+1 " + (MapManager.main.outerTilesLength-1).ToString());
                
                newPosition = (MapManager.main.outerTilesLength-1) + overlapSpots;
                Debug.Log("New position: " + (MapManager.main.outerTilesLength-1).ToString());
            }
            

        }

        if(newPosition >= MapManager.main.outerTilesLength && !middle){
            // If this condition is met, the player has completed a loop and is now trying to access a position thats not on the board
            // We correct this by setting the position to the difference between the max outer tile index and the projected position
            newPosition -= MapManager.main.outerTilesLength;
            Player.current.AddLap(); // Adding a lap to the player's data
        }
        Player.current.SetNewPosition(newPosition);
        currentTurnState = 2;
        CheckCards();

    }



    // IENumerator PostMoveNumerator(){

    // }
}
