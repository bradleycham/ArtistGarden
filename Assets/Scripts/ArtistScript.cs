using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtistScript : MonoBehaviour
{
    Transform player;
    public Object art;
    public Transform artSpawn;

    public float HydrationMeter = 30f; 
    public float HydrationMeterMax = 30f; 
    public float dehydrationPerSecond;
    public float hydrationPerSecond = 5;
    public bool watering = false;

    public float fadeOutTime = 2f;
    public float BurnoutMeter;
    public float SanityMeter;

    public Renderer artistRenderer;
    public Renderer potRenderer;
    public Material greenArtist;
    public Material brownArtist;

    bool dead;
    public ParticleSystem dustPS;
    public Material plantFadeOut;
    public Material potFadeOut;
    public Material invisible;

    public GameObject progressBar;
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
        if (HydrationMeter > HydrationMeterMax && !dead)
        { 
            HydrationMeter = HydrationMeterMax; 
        }
        if (!watering && !dead)
        {
            HydrationMeter -= dehydrationPerSecond * Time.deltaTime;
        }

        if (HydrationMeter <= 0f && !dead)
        {
            dead = true;
            StartCoroutine(FadeOutOfExistance());
            Destroy(progressBar);
        }
        else if(!dead)
            artistRenderer.material.color = Color.Lerp(greenArtist.color, brownArtist.color, 1f - (HydrationMeter / HydrationMeterMax));

        watering = false;
    }
    // Update is called once per frame
    void Update()
    {
        ManageHydration();
        ManageProgress();
    }

    void ManageProgress()
    {
        if (!dead)
        {
            barProgress += Time.deltaTime * progPerSecond;
            if (barProgress >= barMax)
            {
                barProgress = 0f;
                GameObject newArt = (GameObject)Instantiate(art, artSpawn.position, artSpawn.rotation);
                Vector3 dir = player.position - transform.position;
                dir = dir.normalized;
                newArt.GetComponent<Rigidbody>().AddForce(dir * 100f);
            }
            bar.UpdateProgress(barProgress / barMax);
        }
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

    IEnumerator FadeOutOfExistance()
    {
        float timer = 0f;

        artistRenderer.material = plantFadeOut;
        potRenderer.material = potFadeOut;
        Material tempPlantMat = artistRenderer.material;
        Material tempPotMat = potRenderer.material;
        dustPS.Play();
        while (timer <= fadeOutTime)
        {
            timer += Time.deltaTime;
            artistRenderer.material.color = Color.Lerp(tempPlantMat.color, invisible.color, (timer / fadeOutTime));
            potRenderer.material.color = Color.Lerp(tempPotMat.color, invisible.color, (timer / fadeOutTime));

            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(this.gameObject);
    }

}
