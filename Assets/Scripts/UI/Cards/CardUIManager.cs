using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIManager : MonoBehaviour
{
    public static CardUIManager main { get; private set;}
    [SerializeField] private TopHUDScript topHUD;
    [SerializeField] private MainCardUIPanel mainCardHUD;
    public Color32[] cardColors;
    public Sprite[] boostSprites;
    public Sprite[] passiveSprites;
    public Sprite[] sabotageSprites;
    public Sprite[] chanceSprites;
    public Sprite[] specialSprites;
    public Sprite unknownSprite;

    // Start is called before the first frame update
    void Awake()
    {
        if(main == null){ main = this; }
        else if(main != this ){ Destroy(this);}
    }

    public void SetCardUI(){
        mainCardHUD.SetCardUI();
        mainCardHUD.CardButtonRefresh();
        topHUD.UpdateElements();
    }

    public void SetUIRetract(bool retract, float targetTime = 1){
        topHUD.SetPos(retract, targetTime);
        mainCardHUD.SetPos(retract, targetTime);
    }

    public void QuickToggleUIRetract(){
        topHUD.SetPos(!topHUD.retracted);
        mainCardHUD.SetPos(!topHUD.retracted);
    }

    public void Initialize(){
        topHUD.SetupElements();
    }
}
