using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 pos1, pos2;
    [SerializeField] private float animSpeed, timer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(pos1, pos2, 0.5f * Mathf.Sin(timer) + 0.5f);
        timer += animSpeed*Time.deltaTime;
    }
}
