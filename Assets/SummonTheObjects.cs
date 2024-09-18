using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonTheObjects : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform spanwPoint, ground;
    [SerializeField] private Vector3 initPos;
    [SerializeField] private int frameCount, counter;
    [SerializeField] private float timer, speed, depth;

    // Start is called before the first frame update
    void Start()
    {
        initPos = ground.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(counter == 0){

            GameObject summon = Instantiate(prefab, transform);
            summon.transform.SetParent(null);
            counter = frameCount;
        }
        else{
            counter--;
        }

        timer +=   speed * Time.fixedDeltaTime;

        ground.position = initPos + new Vector3(0, depth * Mathf.Sin(timer), 0);

    }
}
