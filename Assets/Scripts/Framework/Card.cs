using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card
{
    /* --- Welcome to the base Card class --- 

        This is where the core structure for cards lie.
        
        While there is a constructor defined, cards in the game will be defined in their own child classes
        Basically, card types will be their own scripts, but they will "inherit" the methods marked as virtual.
        This means that a class which marks this base class as their "parent" will automatically have these virtual methods implemented without needing to write them at all!
        However, to have their own unique functionality, they will need to still write them and mark them with the "override" keyword.
        This allows other scripts to just know them as Card classes, but when calling the methods of that card class they will instead be executing the code defined in the child class

        I really try to explain this its hard lol sorry

        This base class declares variables and methods that the child classes will define
    */


    public Sprite cardSprite {get; protected set;} // The image for this card to use
    public string title {get; protected set;} // The title and description text for the card
    public string description {get; protected set;} // The title and description text for the card
    protected int type, index; // The indexes which specify the card type itself
    public int retireMode {get; protected set;}
    public bool isActive {get; protected set;}
    public bool isRetired {get; protected set;}
    protected Player host;


    public Card(Player _host, int _type, int _index){
        host = _host;
        type = _type;
        index = _index;
    }

    // This constructor is what the child classes will reference
    // All it needs is the player which called it, which in most cases is just the current player
    public Card(Player _host){
        host = _host;
    }

    public Card(Player _host, int _type){
        host = _host;
        type = _type;
        index = 0;
        title = "Unspecified Card";
        description = "Default Card Object";
    }

    // These will be overridden by the child classes
    public virtual void Use(){
        // When the Player decides to use the card
        Debug.Log("Default Card Use Function | Not Implemented for this card.");
        isActive = true;
    }
    public virtual void Initialize(){
        // When the Player first picks up the card
        Debug.Log("Default Card Initialize Function | Not Implemented for this card.");
    }
    public virtual void Passive(){
        // Called after every move of the player -- I just realized when documenting this that this renders another function in FastTrack useless lol, I may fix that
        // Or i may delete this entirely
        Debug.Log("This card has no passive function");
    }
    public virtual void Retire(){
        // When the card has completed its lifespan and is ready to be thrown into the shadow realm
        isActive = false;
        isRetired = true;
    }

    /* These methods are static, and are only used to declare new instances of child card classes 
        Map tiles will decide which of these functions to call based on its type, and in all normal tile cases declare a random card of that type */
    public static Card newBoostCard(Player _host, int index = -1) {  // Pass the child constructor of new cards depending on their index
        
        if(index == -1) {index = Random.Range(0, 2);}
        
        switch (index) {
            case 0:
                return new ThreeSpotBoostCard(_host);
            case 1:
                return new SabotageDefender(_host);
            default:
                break;
        }
        return null;
    }
    public static Card newPassiveCard(Player _host, int index = -1){
        
        if(index == -1) {index = Random.Range(0, 1);}
        
        switch (index) {
            case 0:
                return new BackTrack(_host, Random.Range(0, 2));
            case 1:
                return new SabotageDefender(_host);
            default:
                break;
        }
        return null;

    }
    public static Card newChanceCard(Player _host, int index = -1){
        
        if(index == -1) {index = Random.Range(0, 1);}
        
        switch (index) {
            case 0:
                return new LuckyRoll(_host);
            case 1:
                return new SabotageDefender(_host);
            default:
                break;
        }
        return null;

    }
    public static Card newSabotageCard(Player _host, int index = -1){
        
        if(index == -1) {index = Random.Range(0, 1);}
        
        switch (index) {
            case 0:
                return new BackTrack(_host, Random.Range(0, 10));
            case 1:
                return new SabotageDefender(_host);
            default:
                break;
        }
        return null;

    }
    public static Card newSpecialCard(Player _host, int index = -1){
        
        if(index == -1) {index = Random.Range(0, 1);}

        switch (index) {
            case 0:
                return new ThreeSpotBoostCard(_host);
            case 1:
                return new SabotageDefender(_host);
            default:
                break;
        }
        return null;

    }
}
