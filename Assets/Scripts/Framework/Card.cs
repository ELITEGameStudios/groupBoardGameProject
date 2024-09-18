using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card
{
    protected static string[][] names = new string[][]{ // Names of all possiible cards
        new string[]{
            "FastTrack",
            "SabotageGuard",
        },
        new string[]{
            "PassiveBoost"
        },
        new string[]{
            "LuckyRoll"
        },
    };

    protected static string[][] descriptions = new string[][]{ // Descriptions of all possiible cards
        new string[]{
            "Advance your character by an additional three spots!",
        },
        new string[]{
            "Advance your character by an extra tile each time"
        },
        new string[]{
            "Gain another roll if your first roll is under 4.\nOtherwise, backtrack by what you rolled"
        },
    };
    // The above two lists may not be nessecary depending on how cards are implemented

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

        Debug.Log("your life is NOTHING.");

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

        Debug.Log("your life is NOTHING.");

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

        Debug.Log("your life is NOTHING.");

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

        Debug.Log("your life is NOTHING.");

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

    public virtual void Use(){
        Debug.Log("Default Card Use Function | Not Implemented for this card.");
        isActive = true;
        //Code to play use animation
    }
    public virtual void Initialize(){
        Debug.Log("Default Card Initialize Function | Not Implemented for this card.");
        //Code to play init animation
    }
    public virtual void Passive(){
        Debug.Log("This card has no passive function");
        //Code to play passive animation
    }
    public virtual void Retire(){
        //Code to play retire animation
        isActive = false;
        isRetired = true;
    }
}
