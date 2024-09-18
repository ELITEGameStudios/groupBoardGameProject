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
    
    public void OnEndOfTurn(){
        foreach (Card card in activeCards)
        {
            if(card.retireMode == 0){
                card.Retire();
            }
        }
    }
}
