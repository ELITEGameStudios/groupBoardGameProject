using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TileObjectUI : MonoBehaviour
{
    [SerializeField] private TMP_Text tileIndexText, tileTypeText, tileCardTypeText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private string playerName;
    // Start is called before the first frame update
    public void SetIndexText(int index)
    {
        tileIndexText.text = index.ToString();
    }
    public void SetTypeText(string type)
    {
        tileTypeText.text = type.ToString();
    }
    public void SetCardTypeText(int type)
    {
        switch (type){
            case 0:
                tileCardTypeText.text = "BOOST";
                break;
            case 1:
                tileCardTypeText.text = "PASSIVE";
                break;
            case 2:
                tileCardTypeText.text = "CHANCE";
                break;
            case 3:
                tileCardTypeText.text = "SABOTAGE";
                break;
            case 4:
                tileCardTypeText.text = "SPECIAL";
                break;
        }
    }
}
