using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : MonoBehaviour, ITurnSwitchListener
{
    public static CardSystem main { get; private set;}

    public int usedCardsThisTurn = 0;
    public int maxCardsPerTurn = 1;

    

    public List<Card> activeCards;

    // Start is called before the first frame update
    void Awake(){
        if(main == null){ main = this; }
        else if(main != this ){ Destroy(this);}

        activeCards = new List<Card>();
    }

    public void UseCard(Card card){
        if(card == null || !CanUseCard){ return;}
        card.Use();
        activeCards.Add(card);
        if(!(card is SabotageDefender) || !(card is BaseStealer)){usedCardsThisTurn++;}

    }

    public void UseCard(int slot){
        Card card = Player.current.currentDeck[slot];
        if(card == null || !CanUseCard){ return;}
        card.Use();
        activeCards.Add(card);
        if(card.type != 1){usedCardsThisTurn++;}
        CardUIManager.main.SetCardUI();
    }

    public bool CanUseCard {get {return usedCardsThisTurn < maxCardsPerTurn;}}

    public void TriggerPostMoveFunction(){
        DestroyRetiredCards();
    }
    public void DestroyRetiredCards(){
        foreach (Player player in Player.players){
            for (int i = 0; i < player.currentDeck.Count; i++)
            {
                Card card = player.currentDeck[i];
                if(card == null) { continue; }
                
                if(card.isRetired){
                    player.currentDeck[i] = null;
                    Debug.Log("Retired player " + player.name + " " + i.ToString());
                }
            }
        }

        CardUIManager.main.SetCardUI();
    }


    public List<T> UnpackActiveCards<T>(){
        List<T> unpackedList = new List<T>();
        foreach (Card card in activeCards){
            if (card is T listener)
            {unpackedList.Add(listener); }
        }

        Debug.Log("This should be working with a return list length of " + unpackedList.Count);
        return unpackedList;
    }

    public void OnNextTurn(Player player){
        usedCardsThisTurn = 0;
    }
}
