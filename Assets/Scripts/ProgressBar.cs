using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform progressBar;
    public Transform start;
    public Transform end;
    [Header("Particle System")]
    public Transform particleEnd;
    public Transform PSTrans;
    Transform cam;

    private void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
        
    }
    private void Update()
    {
        transform.LookAt(cam);
        
    }

    public void UpdateProgress(float progress)
    {
        progressBar.localScale = Vector3.Lerp(start.localScale, end.localScale, progress);
        progressBar.position = Vector3.Lerp(start.position, end.position, progress);
        PSTrans.position = Vector3.Lerp(start.position, particleEnd.position, progress);
    }
}
