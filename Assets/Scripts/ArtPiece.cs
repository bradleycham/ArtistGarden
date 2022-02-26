using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtPiece : MonoBehaviour
{
    bool lockOn = false;
    Transform player;
    public Rigidbody rb;
    Vector3 dir;
    public float speed;
    public float speedInc;
   // Start is called before the first frame update
   void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lockOn)
        {
            rb.useGravity = false;
            dir = player.position - transform.position;
            dir = dir.normalized;
            dir *= speed;
            rb.velocity = dir;
            speed += speedInc;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lockOn = true;
            player = other.transform;
        }
    }
}
