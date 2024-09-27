using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundCardSprite : MonoBehaviour
{
    [SerializeField] private Image spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer.color = Color.HSVToRGB(0, 0, Random.Range(0.4f, 0.8f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
