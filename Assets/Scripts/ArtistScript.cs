using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtistScript : MonoBehaviour
{

    public float HydrationMeter; 
    public float HydrationMeterMax; 
    float dehydrationPerSecond;
    public float hydrationPerSecond;
    bool watering = false;

    public float BurnoutMeter;
    public float SanityMeter;

    public Renderer engineBodyRenderer;
    //public Material artistStartColor;
    public Material greenArtist;
    public Material brownArtist;

    // Start is called before the first frame update
    void Start()
    {
        dehydrationPerSecond = Random.Range(0.5f, 1f);

        //StartCoroutine(ChangeEngineColour());
    }

    // Update is called once per frame
    void Update()
    {
        if (HydrationMeter > HydrationMeterMax)
            HydrationMeter = HydrationMeterMax;
        if(!watering)
        {
            HydrationMeter -= dehydrationPerSecond * Time.deltaTime;
        }
        //if(HydrationMeter >= HydrationMeterMax)
          //  engineBodyRenderer.material.color = artistStartColor.color;
        engineBodyRenderer.material.color = Color.Lerp(greenArtist.color, brownArtist.color, 1f - (HydrationMeter / HydrationMeterMax));
        if (HydrationMeter < 0f)
            Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water") && HydrationMeter <= HydrationMeterMax)
        {
            Debug.Log("Watering");

            watering = true;
            HydrationMeter += hydrationPerSecond * Time.deltaTime;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Debug.Log("NotWatering");
            watering = false;
        }
    }
}
