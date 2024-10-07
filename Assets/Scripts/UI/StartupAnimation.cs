using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartupAnimation : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private string[] prompts;
    [SerializeField] private Image image;
    [SerializeField] private float timer, time;
    [SerializeField] private Color textColor, imageColor;

    // Start is called before the first frame update
    void Awake(){
        text.text = prompts[Random.Range(0, prompts.Length)];
        
        textColor = Color.HSVToRGB(Random.value, 0.75f, 1f);
        text.color = textColor;
        time = timer;

        StartCoroutine(MainCoroutine());
    }


    IEnumerator MainCoroutine(){
        yield return new WaitForSeconds(2.5f);


        while(time > 0){

            image.color =  Color.Lerp(Color.clear, imageColor, time/timer);
            text.color =  Color.Lerp(Color.clear, imageColor, time/timer);
            time -= Time.deltaTime;
            yield return null;
        }

        Destroy(text.gameObject);
        Destroy(gameObject);
    }
}
