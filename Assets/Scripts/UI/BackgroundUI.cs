using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BackgroundUI : MonoBehaviour
{
    [SerializeField] private GameObject textFieldPrefab;
    [SerializeField] private Transform namesListTf;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Button playButton;
    [SerializeField] private Image glowEffect;
    [SerializeField] private float glowEffectHue;
    [SerializeField] private Color alphaColor;
    

    // Start is called before the first frame update
    public void submitInput(){
        if (GameManager.main.playerNames.Count >= 1){ // Allows the play button to appear once two players are registered after this method's lifespan
            playButton.interactable = true;
            if(GameManager.main.playerNames.Count >= 12){ // Restricts any more than 12 players in the game
                input.interactable = false;
                PopupUIManager.main.DisplayMessage("You have reached the 12 player maximum", Color.cyan, 2);
                return;
            }
        }

        GameManager.main.playerNames.Add(input.text);
        
        Text newNameDisplay = Instantiate(textFieldPrefab, namesListTf).transform.GetChild(0).GetComponent<Text>();
        newNameDisplay.text = input.text;

    }

    void Awake(){
        glowEffectHue = Random.value;
    }
    void Update(){
        glowEffect.color = Color.Lerp(alphaColor, Color.HSVToRGB(glowEffectHue, 1, 1), 0.5f);
        glowEffectHue += 0.02f * Time.deltaTime;
        if(glowEffectHue >= 1){glowEffectHue = 0;};
    }
}
