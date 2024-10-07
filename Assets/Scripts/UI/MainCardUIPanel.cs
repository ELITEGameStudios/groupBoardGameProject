using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCardUIPanel : MonoBehaviour
{

    [SerializeField] private Transform retractedPos, normalPos;
    [SerializeField] private Vector3 capturedPos;
    [SerializeField] private bool retracted, retracting, moving;
    [SerializeField] private float timer, animTime;
    [SerializeField] private CardUI[] cardUIPanels;
    [SerializeField] private Button[] useButton;
    [SerializeField] private TMP_Text[] useButtonText;
    [SerializeField] private Color retirePassiveColor, useCardColor, unavailableColor;



    
    public void SetCardUI(){
        for (int i = 0; i < Player.current.currentDeck.Count; i++) {

            if(Player.current.currentDeck[i] != null)
            { cardUIPanels[i].SetCard(Player.current.currentDeck[i]); } 
            
            else { cardUIPanels[i].SetCard(); }
        }
    }

    public void CardButtonRefresh(){

        for (int i = 0; i < useButton.Length; i++){
            if(Player.current.currentDeck[i] == null){
                useButtonText[i].text = "";
                useButton[i].interactable = false;
                useButton[i].GetComponent<Image>().color = unavailableColor;
                continue;
            }
            if(Player.current.currentDeck[i] is BaseStealer || Player.current.currentDeck[i] is SabotageDefender){ // If the corresponding card is a passive
                useButtonText[i].text = "RETIRE";
                useButton[i].interactable = true;
                useButton[i].GetComponent<Image>().color = retirePassiveColor;
                continue;
            }
            if(!CardSystem.main.CanUseCard){ // If cards cannot be used
                useButtonText[i].text = "";
                useButton[i].interactable = false;
                useButton[i].GetComponent<Image>().color = unavailableColor;
            }
            else{ // If cards CAN be used at this time
                useButtonText[i].text = "USE";
                useButton[i].interactable = true;
                useButton[i].GetComponent<Image>().color = useCardColor;
            }
        }
    }

    void Update(){
        if(moving){
            if(timer > 0){
                if(retracting){
                    transform.position = Vector3.Lerp( retractedPos.position, capturedPos,
                        Mathf.Pow(Mathf.Sin(Mathf.PI* ( timer / 2 * animTime)), 2)); }
                else{
                    transform.position = Vector3.Lerp( normalPos.position, capturedPos, 
                        Mathf.Pow(Mathf.Sin(Mathf.PI* ( timer / 2 * animTime)), 2)); }

                timer -= Time.deltaTime;
            }
            else{
                transform.position = retracting ? retractedPos.position : normalPos.position;
                moving = false;
                retracted = retracting;
            }
        }
    }

    public void SetPos(bool retract, float targetTime = 1){
        if(
            (retracted == retract && !moving) || 
            retracting == retract || 
            (!CardSystem.main.CanUseCard && !retract)
        ) {return;}


        retracting = retract;
        capturedPos = transform.position;
        animTime = targetTime;
        timer = targetTime;
        moving = true;
    }

}
