using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundCardSprite : MonoBehaviour
{
    [SerializeField] private Image spriteRenderer;
    [SerializeField] private float rate;

    // Start is called before the first frame update
    void Awake()
    {
        // transform.localPosition = Vector3.zero;
        spriteRenderer.color = Color.HSVToRGB(0, 0, Random.Range(0.4f, 0.8f));
        rate = Random.Range(-45, 45);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rate * Time.deltaTime);
    }
}
