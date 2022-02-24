using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour
{
    // Start is called before the first frame update
    //public Transform sprinkler;
    public Transform startLoc;
    public Transform endLoc;
    float timer;
    public float duration;
    bool flip;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (flip)
            transform.rotation = Quaternion.Lerp(endLoc.rotation, startLoc.rotation, timer / duration);
        else
            transform.rotation = Quaternion.Lerp(startLoc.rotation, endLoc.rotation, timer / duration);
        if(timer >= duration)
        {
            flip = !flip;
            timer = 0f;
        }
    }
}
