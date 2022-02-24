using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public ParticleSystem PS;
    public CapsuleCollider col;
    bool water;
    // Start is called before the first frame update
    void Start()
    {
        water = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            water = !water;
            col.enabled = water;
            if (water)
                PS.Play();
            else
                PS.Stop();
            Debug.Log("Click: " + water);
        }

    }
}
