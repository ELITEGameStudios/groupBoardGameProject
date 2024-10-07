using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLoopManager : MonoBehaviour
{
    bool requiresMiddleCondition;
    [SerializeField] private bool twoDice;
    [SerializeField] private bool extraTurn, middleCondition = false, requiresRoll = true;
    [SerializeField] private int projectedPosition, spotsToMiddleConnector;
    [SerializeField] private GameObject middleChoiceUI;
    public int currentTurnState = 0; // 0 is pre-roll, 1 is during initial move, 2 is post first move
    public int rollOutcome = 0; // 0 is pre-roll, 1 is during initial move, 2 is post first move
    public static TurnLoopManager main { get; private set;}
    [SerializeField] private Queue<AdditionalMovementQueueObject> sabotageQueue;

    void Awake() {
        if(main == null){ main = this; }
        else if(main != this ){ Destroy(this);}
        sabotageQueue = new();
    }

    public void StartGame(){
        StartCoroutine("MainLoopCoroutine"); // Starts the main game loop

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

    public void PassMiddleCondition(bool condition){
        // Disable UI
        if(requiresMiddleCondition){
            requiresMiddleCondition = false;
            middleCondition = condition;

            middleChoiceUI.SetActive(false);
        }
    }

    public void DoRollSystem(){ // Called by Unity UI to roll the dice
        requiresRoll = false;
    }
    public void QueueAdditionalMovement(Player player, int offset, int queueOldPosition, bool punch = false)
    {
        sabotageQueue.Enqueue(new AdditionalMovementQueueObject(player, offset, queueOldPosition, punch));
    }

    public bool AtMiddle {
        get {
            return Player.current.boardPosition >= MapManager.main.outerTilesLength; // If the players board position is over the outer tiles position BEFORE the roll, then the player is on a middle tile
        }
    }
    public bool PlayerAtMiddle(Player player) {
        return player.boardPosition >= MapManager.main.outerTilesLength; // If the players board position is over the outer tiles position BEFORE the roll, then the player is on a middle tile
    }

    void ProjectPosition(){
        projectedPosition = Player.current.boardPosition + rollOutcome; // The position the player should be after this move has been completed
        // DoUiFunction();
        
        if(AtMiddle){
            // MovePlayerPosition(projectedPosition, true, rollOutcome); // Feeds the projected pos to the movePlayer function
            currentTurnState = 2;
            return;
        }

        if(TestForMiddleCondition(Player.current.boardPosition, projectedPosition)){
            spotsToMiddleConnector = MapManager.main.GetSpotsPastMiddleConnector(projectedPosition);
            PromptMiddleCondition();
            currentTurnState = 2;
            
            return;
        }

        // MovePlayerPosition(projectedPosition, false); // Feeds the projected pos to the movePlayer function
        currentTurnState = 2;
        return;

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
    public void MovePlayerPosition(int newPosition, Player player, bool middle = false, int roll = -1){
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
            else if(!PlayerAtMiddle(player)) { // This means the player is currently on the normal board, and transitioning to the middle
                int overlapSpots = MapManager.main.GetSpotsPastMiddleConnector(newPosition);
                newPosition = (MapManager.main.outerTilesLength-1) + overlapSpots;
            }
            else if(newPosition < MapManager.main.outerTilesLength && PlayerAtMiddle(player)){ // Player is being sabotaged back from the middle tiles
                int difference = newPosition - player.boardPosition;

                newPosition = MapManager.main.GetMiddleTile().index + (difference+1 - (MapManager.main.outerTilesLength - player.boardPosition));
            }

        }
        if(newPosition >= MapManager.main.outerTilesLength && !middle){
            // If this condition is met, the player has completed a loop and is now trying to access a position thats not on the board
            // We correct this by setting the position to the difference between the max outer tile index and the projected position
            newPosition -= MapManager.main.outerTilesLength;
            player.EndGame(); // Adding a lap to the player's data
        }
        if(newPosition < 0 && !middle){ newPosition = 0; }

        player.SetNewPosition(newPosition);
        currentTurnState = 2;
        // CheckCards();
    }

    public void GiveExtraTurn(){
        extraTurn = true;
    }

    public void ChangeRollOutcome(int newOutcome){
        rollOutcome = newOutcome;
    }
    
    public void Roll(){
        currentTurnState = 1;
        // Generate the outcome of the dice roll
        rollOutcome = Random.Range(1, twoDice ? 13 : 7); // Generates a randomized outcome with ranges dependant on if the user has a second dice or not
        return;
        // rollOutcome = 1;
    }

    IEnumerator RollAnimationCoroutine(){
        float timer = 2;
        while (timer > 0){
            
            GlobalUI.main.GetRollOutcomeText().text = Random.Range(0, twoDice ? 13 : 7).ToString();
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    bool PlayerMovingForwards( Player player, int playerOldPos, int playerNewPos) { // With the tile system and order in place, its hard to tell if the player is going forwards or backwards without these conditions
        if( player == null ){return false;}
        if(playerNewPos > playerOldPos){ return true; }
        if(PlayerAtMiddle(player) && playerNewPos >= MapManager.main.GetMiddleTile(false).index){ return true; }
        // if(playerNewPos >= MapManager.main.GetMiddleTile(false).index){ return true; } for laps
        return false;
    }

    IEnumerator MoveAnimationCoroutine(int oldPosition, int newPosition, Player player, bool punch = false){ // Responsible for the animation of players moving forwards or backwards
        List<Vector2> positions = new List<Vector2>();
        bool forwards = newPosition > oldPosition;
        punch = true; // A tentative solution for some problems the animator is facing at the moment

        if(!punch){

            if(PlayerMovingForwards(player, oldPosition, newPosition)){ // If the player is moving forwards
                if(newPosition > MapManager.main.outerTilesLength && oldPosition < MapManager.main.outerTilesLength){ // If the player is transitioning to the middle tiles
                    Debug.Log("FIRE IN THE HOLEE");
                    for (int i = oldPosition; i <= MapManager.main.GetMiddleTile().index; i++)
                    { positions.Add(MapManager.main.GetTile(i).gameObject.transform.position); } 
                    
                    for (int i = MapManager.main.outerTilesLength; i <= newPosition; i++)
                    { positions.Add(MapManager.main.GetTile(i).gameObject.transform.position); } 

                }
                else if(oldPosition > MapManager.main.outerTilesLength && newPosition < MapManager.main.outerTilesLength){ // If the player is transitioning from the middle back to the main course
                    Debug.Log("WATER ON THE HILL");
                    
                    for (int i = oldPosition; i < MapManager.main.totalTiles; i++)
                    { positions.Add(MapManager.main.GetTile(i).gameObject.transform.position); } 
                    
                    for (int i = MapManager.main.GetMiddleTile(false).index; i <= newPosition; i++)
                    { positions.Add(MapManager.main.GetTile(i).gameObject.transform.position); } 

                }
                else{ // There are no transitions happening this move
                    Debug.Log("AIR DETECTED");
                    for (int i = oldPosition; i <= newPosition; i++)
                    { positions.Add(MapManager.main.GetTile(i).gameObject.transform.position); } 
                }
            }
            else{ // The player is moving backwards

                if(newPosition < MapManager.main.outerTilesLength && oldPosition > MapManager.main.outerTilesLength){ // If the player is transitioning from the middle to the outer tiles
                    
                    for (int i = oldPosition; i > MapManager.main.GetMiddleTile().index; i--)
                    { positions.Add(MapManager.main.GetTile(i).gameObject.transform.position); } 
                    
                    for (int i = MapManager.main.GetMiddleTile().index; i >= newPosition; i--)
                    { positions.Add(MapManager.main.GetTile(i).gameObject.transform.position); } 

                }
                else{
                    for (int i = oldPosition; i >= newPosition; i--)
                    { positions.Add(MapManager.main.GetTile(i).gameObject.transform.position); } 
                }

            }
        }
        else{
            Debug.Log("PUNCHED!");
            positions.Add(MapManager.main.GetTile(oldPosition).gameObject.transform.position);
            positions.Add(MapManager.main.GetTile(newPosition).gameObject.transform.position);
        }

        
        Vector2 newPositionVector = MapManager.main.GetTile(newPosition).gameObject.transform.position;
        player.gameObject.transform.position = MapManager.main.GetTile(oldPosition).gameObject.transform.position;
        
        float timer = 0;
        float stepTime = 0.15f;


        for (int i = 0; i < positions.Count; i++)
        {
            Vector2 currentPos = player.gameObject.transform.position;
            timer = stepTime;
            while (timer > 0)
            {
                player.gameObject.transform.position = Vector2.Lerp(positions[i], currentPos, 0.5f * Mathf.Sin(Mathf.PI * (timer/stepTime - 0.5f)) + 0.5f); 
                timer -= Time.deltaTime;
                yield return null;
            }
            player.gameObject.transform.position = positions[i];
        }
        player.gameObject.transform.position = newPositionVector;
    }

    IEnumerator CheckOddMovementCoroutine(){ 

        /*  
            This system is in place to queue, process, and animate any movement that is not the current players roll.

            Objects outside of the TurnLoopManager will be able to queue any extra moves done to any player within a turn
            They do this by calling QueueAdditionalMovement() which creates an AdditionalMovementQueueObject and adds that to the queue
            This coroutine is executed at the beginning and end of the main loop to ensure that these moves are done at the right time

        */

        AdditionalMovementQueueObject nextSabotageEvent; 
        sabotageQueue.TryPeek(out nextSabotageEvent); // Tries to retrieve the next queued object
        
        while(nextSabotageEvent != null){// Executes as long as there is another object in the queue
            CardUIManager.main.SetUIRetract(true);
            
            MovePlayerPosition(nextSabotageEvent.oldPosition + nextSabotageEvent.offset, nextSabotageEvent.player, PlayerAtMiddle(nextSabotageEvent.player)); // Moves the player's position accordingly 
            yield return StartCoroutine(MoveAnimationCoroutine(nextSabotageEvent.oldPosition, nextSabotageEvent.player.boardPosition, nextSabotageEvent.player, nextSabotageEvent.punch)); // Plays the movement animation  
            
            sabotageQueue.Dequeue();
            sabotageQueue.TryPeek(out nextSabotageEvent); // Tries to retrieve the next queued object. Will break the loop if the queue has finished
            if(nextSabotageEvent == null){CardUIManager.main.SetUIRetract(false);}
        }
    }

    public void StopMainLoop(){
        StopCoroutine("MainLoopCoroutine");
    }


    IEnumerator MainLoopCoroutine(){
        

        // As long as this loop continues, the game will continue... forever
        while(true){
            // IEnumerator
            /* Preroll phase */

            Debug.Log(Player.players.Count);
            Debug.Log(GameManager.main.currentPlayerIndex);

            CustomEventSystem.TriggerNewTurn();
            CardUIManager.main.SetCardUI();
            CardUIManager.main.SetUIRetract(false);

            int oldPosition = Player.current.boardPosition;

            // Ensures that a player will need to provide input before proceeding loop repeats            
            requiresRoll = true;
            while(requiresRoll) { // Halts execution until the player wants to roll
                yield return StartCoroutine(CheckOddMovementCoroutine());
                yield return null; 
            } 
            CardUIManager.main.SetUIRetract(true);
            
            yield return StartCoroutine("RollAnimationCoroutine"); // Triggers the roll animation

            /* Roll phase */
            Roll(); // Rolls and generates outcome
            GlobalUI.main.GetRollOutcomeText().text = rollOutcome.ToString(); // Sets UI To show Roll 
            CustomEventSystem.TriggerPrePlayerMove(); // Any logic to modify the outcome or game events before projecting position
            ProjectPosition(); // Projects the new position of the player and prompts for middle condition if needed


            /* Middle choice and player movement phase */
            if(requiresMiddleCondition){ // Checks if the middle condition is required
                while (requiresMiddleCondition){ yield return null; } // Stops execution until the player decides which path to choose
                // When PassMiddleConditionNew is called, it will set requiresMiddleCondition to false and set the middlePosition variable accordingly
                MovePlayerPosition(projectedPosition, Player.current, middleCondition, rollOutcome); // Moves the player through the chosen path 
            }
            else{
                MovePlayerPosition(projectedPosition, Player.current, AtMiddle, rollOutcome); // A standard roll and move if the ccondition is not required
            }
            yield return StartCoroutine(MoveAnimationCoroutine(oldPosition, Player.current.boardPosition, Player.current));
            
            /* Post movement phase */

            // Custom logic at this phase including card effects
            CustomEventSystem.TriggerPosInitMove();
            
            // Background card management
            CheckPostMoveCards();
            Player.current.PickupNewTileCard();

            yield return StartCoroutine(CheckOddMovementCoroutine()); 

            // Extra turn handler
            if(!extraTurn){  GameManager.main.NextTurn(); }
            else{ extraTurn = !extraTurn; Debug.Log("RAHHHH"); } 
            
            yield return new WaitForSeconds(1.5f);
        }
    }

}

public class AdditionalMovementQueueObject
{
    public Player player;
    public int offset, oldPosition;
    public bool punch;
    public AdditionalMovementQueueObject(Player player, int offset, int oldPosition, bool punch = false){
        this.player = player;
        this.offset = offset;
        this.oldPosition = oldPosition; 
        this.punch = punch; 
    }
}