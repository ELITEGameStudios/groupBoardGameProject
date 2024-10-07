using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopHudElement : MonoBehaviour
{
    

    [SerializeField] private CardUI[] cards;
    [SerializeField] private Text playerName;
    [SerializeField] private Image background, playerSprite;
    [SerializeField] private Color onTurnColor, normalColor;



    // Start is called before the first frame update
    public void SetPlayer(Player player) {
        playerName.text = player.name;

        for (int i = 0; i < 3; i++){
            try {
                cards[i].SetCard(player.currentDeck[i]);
            }
            catch (System.Exception){
                Debug.Log("Index was likely out of range, the TopHudUI object was not set up properly.");
            }
        }

        playerSprite.sprite = player.gameObject.GetComponent<SpriteRenderer>().sprite;
        playerSprite.color = player.gameObject.GetComponent<SpriteRenderer>().color;


        if (Player.current == player){
            background.color = onTurnColor;
        }
        else{
            background.color = normalColor;
        }
    }
}
