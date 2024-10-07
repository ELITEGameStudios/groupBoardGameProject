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

    public string title {get; protected set;} // The title and description text for the card
    public string description {get; protected set;} // The title and description text for the card
    public int type {get; protected set;} // The indexes which specify the card type itself
    public int index {get; protected set;} // The indexes which specify the card type itself
    public bool isActive {get; protected set;}
    public bool isRetired {get; protected set;}
    protected Player host;


    // This constructor is what the child classes will reference
    // All it needs is the player which called it, which in most cases is just the current player
    public Card(Player _host){
        host = _host;
    }

    // These will be overridden by the child classes
    public virtual void Use(){
        // When the Player decides to use the card
        isActive = true;
    }
    public virtual void Initialize(){
        // When the Player first picks up the card
    }

    public virtual void Retire(){
        // When the card has completed its lifespan and is ready to be thrown into the shadow realm
        isActive = false;
        isRetired = true;
        CardSystem.main.DestroyRetiredCards();
    }

    public Sprite GetSprite(){
        Debug.Log(index);
        switch(type){
            case 0: 
                return CardUIManager.main.boostSprites[index];
            case 1:
                return CardUIManager.main.passiveSprites[index];
            case 2:
                return CardUIManager.main.chanceSprites[index];
            case 3:
                return CardUIManager.main.sabotageSprites[index];
            case 4:
                return CardUIManager.main.specialSprites[index];
        }

        return CardUIManager.main.unknownSprite;
    }

    /* These methods are static, and are only used to declare new instances of child card classes 
        Map tiles will decide which of these functions to call based on its type, and in all normal tile cases declare a random card of that type */
    public static Card newBoostCard(Player _host, int index = -1) {  // Pass the child constructor of new cards depending on their index
        return new FastTrackBoostCard(_host);
    }
    public static Card newPassiveCard(Player _host, int index = -1){
        
        if(index == -1) {index = Random.Range(0, 2);}
        
        switch (index) {
            case 0:
                return new BaseStealer(_host);
            case 1:
                return new SabotageDefender(_host);
            default:
                break;
        }
        return null;

    }
    public static Card newChanceCard(Player _host, int index = -1){
        
        return new LuckyRoll(_host);
    }
    public static Card newSabotageCard(Player _host, int index = -1){
        
        return new BackTrack(_host, Random.Range(2, 7));

    }
    public static Card newSpecialCard(Player _host, int index = -1){
        
        if(index == -1) {index = Random.Range(0, 4);}

        switch (index) {
            case 0:
                return new FastTrackBoostCard(_host, true);
            case 1:
                return new SabotageDefender(_host, true);
            case 2:
                return new BaseStealer(_host, true);
            case 3:
                return new BackTrack(_host, Random.Range(6, 15), true);
            default:
                break;
        }
        return null;

    }
}
