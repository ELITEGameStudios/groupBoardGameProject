using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCardsEffect : MonoBehaviour
{
    [SerializeField] private List<GameObject> list;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float offsetTime;
    [SerializeField] private int offsetIterations, currentIterations, height, spread, speed;
    // Start is called before the first frame update
    void Awake()
    {
        list = new List<GameObject>();
        StartCoroutine("Animation");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var obj in list)
        {
            obj.transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }
    }

    IEnumerator Animation(){
        while (true)
        {       
            yield return new WaitForSeconds(offsetTime);
            for (int i = 0; i < 2; i++)
            {
                GameObject newObject = Instantiate(prefab, transform);
                newObject.transform.position = new Vector3(Random.Range(-spread, spread), height + Random.Range(-4.0f, 4.0f));
                newObject.transform.localEulerAngles= new Vector3(0, 0, Random.Range(-45, 45));
                list.Add(newObject);

                if(offsetIterations <= currentIterations){
                    list.RemoveAt(0);
                }
                
            }
            currentIterations++;
        }
    }
}
