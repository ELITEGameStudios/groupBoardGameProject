using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopHUDScript : MonoBehaviour
{
    public List<TopHudElement> elements;
    public GameObject prefab;
    public Transform contentScrollRectTf;
    [SerializeField] private Transform retractedPos, normalPos;
    [SerializeField] private Vector3 capturedPos;
    public bool retracted {get; private set;}
    [SerializeField] private bool retracting, moving;
    [SerializeField] private float timer, animTime;

    void Awake(){
        transform.position = normalPos.position;
    }

    // Update is called once per frame
    public void UpdateElements(){
        for (int i = 0; i < elements.Count; i++){
            elements[i].SetPlayer(Player.players[i]);
        }
    }

    public void SetupElements(){
        elements = new();
        for (int i = 0; i < Player.players.Count; i++){
            elements.Add(Instantiate(prefab, contentScrollRectTf).GetComponent<TopHudElement>());
            elements[i].SetPlayer(Player.players[i]);
        }
        SetPos(retracted);
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
        if(retracting == retract) {return;}
        retracting = retract;
        capturedPos = transform.position;
        animTime = targetTime;
        timer = targetTime;
        moving = true;
    }

}
