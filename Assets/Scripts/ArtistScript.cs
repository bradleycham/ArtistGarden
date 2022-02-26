using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtistScript : MonoBehaviour
{
    Transform player;
    public Object art;
    public Transform artSpawn;

    public float HydrationMeter; 
    public float HydrationMeterMax; 
    float dehydrationPerSecond;
    public float hydrationPerSecond;
    bool watering = false;

    public float BurnoutMeter;
    public float SanityMeter;

    public Renderer artistRenderer;
    public Material greenArtist;
    public Material brownArtist;

    public ProgressBar bar;
    public float barProgress;
    public float barMax = 100f;
    float progPerSecond = 5f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("ThirdPersonPlayer").transform;

        progPerSecond = Random.Range(3f, 5f);
        HydrationMeterMax = HydrationMeter;
        dehydrationPerSecond = Random.Range(0.5f, 1.0f);

        //StartCoroutine(ChangeEngineColour());
    }

    void ManageHydration()
    {
        if (HydrationMeter > HydrationMeterMax)
            HydrationMeter = HydrationMeterMax;
        if (!watering)
        {
            HydrationMeter -= dehydrationPerSecond * Time.deltaTime;
        }

        artistRenderer.material.color = Color.Lerp(greenArtist.color, brownArtist.color, 1f - (HydrationMeter / HydrationMeterMax));
        if (HydrationMeter < 0f)
            Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        ManageHydration();
        ManageProgress();
    }

    void ManageProgress()
    {
        barProgress += Time.deltaTime * progPerSecond;
        if(barProgress >= barMax)
        {
            barProgress = 0f;
            GameObject newArt = (GameObject)Instantiate(art, artSpawn.position, artSpawn.rotation);
            Vector3 dir = player.position - transform.position;
            dir = dir.normalized;
            newArt.GetComponent<Rigidbody>().AddForce(dir * 100f);
        }
        bar.UpdateProgress(barProgress / barMax);
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water") && HydrationMeter <= HydrationMeterMax)
        {
            //Debug.Log("Watering");

            watering = true;
            HydrationMeter += hydrationPerSecond * Time.deltaTime;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            //Debug.Log("NotWatering");
            watering = false;
        }
    }
}
