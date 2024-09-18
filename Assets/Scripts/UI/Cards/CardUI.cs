using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text cardTypeText, cardTitleText, cardDescText;
    // [SerializeField] private  cardTypeText, cardTitleText, cardDescText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetCard(Card card = null){
        if(card != null){
            cardTitleText.text = card.title;
            cardDescText.text = card.description;
        }
        else{
            cardTitleText.text = "None";
            cardDescText.text = "None";
            
        }
    }
}
