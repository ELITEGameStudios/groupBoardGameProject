using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class PlayerObjectUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private string playerName;
    // Start is called before the first frame update
    public void SetName(string _name)
    {
        playerName = _name;
        text.text = playerName;
        SetColor();
    }

    public void SetColor(){
        
        // if(color != null){
        //     spriteRenderer.color = color;
        // }
        // else{
            spriteRenderer.color = Color.HSVToRGB(Random.value, 0.65f, 0.75f );
        // }
    }
}
