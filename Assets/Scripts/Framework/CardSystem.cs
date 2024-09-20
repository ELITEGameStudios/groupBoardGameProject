using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : MonoBehaviour, IEndOfTurnListener
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
        if(card == null || usedCardsThisTurn >= maxCardsPerTurn){ return;}
        card.Use();
        activeCards.Add(card);

    }

    public void UseCard(int slot){
        Card card = Player.current.currentDeck[slot];
        if(card == null || usedCardsThisTurn >= maxCardsPerTurn){ return;}
        card.Use();
        activeCards.Add(card);

    }

    public void TriggerPostMoveFunction(){
        CustomEventSystem.TriggerPosInitMove(true);
        DestroyRetiredCards();
    }
    void DestroyRetiredCards(){
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
    }

    public List<IPosInitialtMoveListener> Unpack(){
        List<IPosInitialtMoveListener> unpackedList = new List<IPosInitialtMoveListener>();
        foreach (Card card in activeCards){
            IPosInitialtMoveListener listener = card as IPosInitialtMoveListener;
            if(listener != null){
                unpackedList.Add(listener);
            }
        }
        return unpackedList;
    }
    
    public void OnEndOfTurn(){
        foreach (Card card in activeCards)
        {
            if(card.retireMode == 0){
                card.Retire();
            }
        }
    }
}
