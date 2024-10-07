using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text cardTypeText, cardTitleText, cardDescText;
    [SerializeField] private Image cardImage;

    public void SetCard(Card card = null){
        if(card != null){
            cardTitleText.text = card.title;
            cardDescText.text = card.description;
            cardImage.sprite = card.GetSprite();

            cardTitleText.color = CardUIManager.main.cardColors[card.type];
            cardTypeText.color = CardUIManager.main.cardColors[card.type];

            switch (card.type){
                case 0:
                    cardTypeText.text = "BOOST";
                    break;
                case 1:
                    cardTypeText.text = "PASSIVE";
                    break;
                case 2:
                    cardTypeText.text = "CHANCE";
                    break;
                case 3:
                    cardTypeText.text = "SABOTAGE";
                    break;
                case 4:
                    cardTypeText.text = "SPECIAL";
                    break;
            }
        }
        else{
            cardTitleText.text = "Unknown";
            cardDescText.text = "";
            cardTypeText.text = "";
            cardImage.sprite = CardUIManager.main.unknownSprite;
            cardTitleText.color = Color.black;
            cardTypeText.color = Color.black;
        }
    }
}
