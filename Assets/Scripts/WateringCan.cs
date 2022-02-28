using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public ParticleSystem PS;
    public CapsuleCollider col;
    public bool water;
    // Start is called before the first frame update
    void Start()
    {
        col.enabled = false;
        water = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WaterOnOff()
    {

        water = !water;

        if (water)
        {
            col.enabled = true;
            PS.Play();
        }
        else
        {
            col.enabled = false;
            PS.Stop();
        }
        Debug.Log("Click: " + water);
        
    }
}
