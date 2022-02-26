using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform sprinkler;
    public Transform startLoc;
    public Transform endLoc;

    public ParticleSystem PS;
    public SphereCollider col;
    float timer;
    public float duration;
    bool flip;
    void Start()
    {
        PS.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (flip)
            sprinkler.rotation = Quaternion.Lerp(endLoc.rotation, startLoc.rotation, timer / duration);
        else
            sprinkler.rotation = Quaternion.Lerp(startLoc.rotation, endLoc.rotation, timer / duration);
        if(timer >= duration)
        {
            flip = !flip;
            timer = 0f;
        }
    }

    public void Sprinkle(bool on)
    {
        if(on)
        {
            PS.Play();
            col.enabled = true;
        }
        else
        {
            PS.Stop();
            col.enabled = false;
        }
    }
}
