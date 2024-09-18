using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIManager : MonoBehaviour
{
    public static CardUIManager main { get; private set;}
    [SerializeField] private List<CardUI> cardUIPanels;


    // Start is called before the first frame update
    void Awake()
    {
        if(main == null){ main = this; }
        else if(main != this ){ Destroy(this);}
    }

    public void SetCardUI(){
        for (int i = 0; i < Player.current.currentDeck.Count; i++)
        {
            if(Player.current.currentDeck[i] != null){
                cardUIPanels[i].SetCard(Player.current.currentDeck[i]);
            } 
            else{
                cardUIPanels[i].SetCard();
            }
        }
    }
}
