using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile
{
    public int index {get; private set;}
    public int cardType {get; private set;}
    public string tileType {get; private set;}
    // Handles middle connections. if this is -1, it does not connect to the middle.
    // Handles middle connections. if this is 0 or 1, it connects to middle endpoint 0 or 1.
    public int middleConnectorType {get; private set;}
    public GameObject gameObject;
    public TileObjectUI graphicsHelper {get; private set;}
    public MapTile(int _index, string _tileType){
        index = _index;
        middleConnectorType = -1;
        tileType = _tileType;
        cardType = -1;
    }

    public MapTile(int _index, string _tileType, int _middleConnectorType){
        index = _index;
        middleConnectorType = _middleConnectorType;
        tileType = _tileType;
        cardType = -1;
    }

    public void SetGameObject(GameObject _gameObject){
        if(gameObject == null){
            gameObject = _gameObject;
        }

        graphicsHelper = gameObject.GetComponent<TileObjectUI>();
        graphicsHelper.SetIndexText(index);
        graphicsHelper.SetTypeText(tileType);
    }

    void Shout(){
        Debug.Log("I EXIST! ID: " + index);
    }

    public void SetTileType(int type){
        if(tileType == "_normal"){
            cardType = type;
            if(graphicsHelper != null){
                graphicsHelper.SetCardTypeText(cardType);
            }
        }
    }
    
    public Card GetNewCard(Player host){
        if(tileType == "_normal" && cardType != -1){
            switch(cardType){
                case 0:
                    return Card.newBoostCard(host);
                case 1:
                    return Card.newPassiveCard(host);
                case 2:
                    return Card.newChanceCard(host);
                case 3:
                    return Card.newSabotageCard(host);
                case 4:
                    return Card.newSpecialCard(host);
                default:
                    break;
            }
        }
        return null;
    }
}