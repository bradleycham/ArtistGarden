using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtistScript : MonoBehaviour
{
    Transform player;
    public Object art;
    public Transform artSpawn;

    // HYDRATION
    public float hydrationMeter = 30f; 
    public float hydrationMeterMax = 30f; 
    public float dehydrationPerSecond;
    public float hydrationPerSecond = 5f;
    public bool watering = false;
    public float fadeOutTime = 2f;

    // ART PROGRESS BAR
    public GameObject progressBar;
    public ProgressBar bar;
    public float barProgress;
    public float barMax = 100f;
    float progPerSecond = 5f;
    float progPerSecStart;

    // BURNOUT
    public float burnoutMeterMax = 30f;
    public float burnoutMeter = 0f;
    public float burnoutPerSecond = 3f;
    public float wateringRelief = 2f;
    public bool burning;
    public ParticleSystem burningVFX;

    public GameObject burnoutBar;
    public ProgressBar bBar;
    float burnoutProductivity = 5f;

    public float sanityMeter;

    public Renderer stemRenderer;
    public Renderer leave01;
    public Renderer leave02;
    public Renderer leave03;
    public Renderer leave04;

    public Renderer potRenderer;
    public Material greenArtist;
    public Material brownArtist;

    public Transform l1start;
    public Transform l2start;
    public Transform l3start;
    public Transform l4start;
    public Transform l1end;
    public Transform l2end;
    public Transform l3end;
    public Transform l4end;

    bool dead;
    public ParticleSystem dustPS;
    public Material plantFadeOut;
    public Material potFadeOut;
    public Material invisible;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("ThirdPersonPlayer").transform;

        progPerSecStart = progPerSecond;
        //progPerSecond = Random.Range(3f, 5f);
        hydrationMeterMax = hydrationMeter;
        dehydrationPerSecond = Random.Range(0.5f, 1.0f);

        //StartCoroutine(ChangeEngineColour());
    }
    
    // Update is called once per frame
    void Update()
    {
        ManageBurnout();
        ManageHydration(); // sets watering to false;
        ManageProgress();
    }
    
    // ART PROGRESS BAR
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

    // HYDRATION
    void ManageHydration()
    {
        if (hydrationMeter > hydrationMeterMax && !dead)
        {
            hydrationMeter = hydrationMeterMax;
        }
        if (!watering && !dead)
        {
            hydrationMeter -= dehydrationPerSecond * Time.deltaTime;
        }

        if (hydrationMeter <= 0f && !dead) // become dead
        {
            dead = true;
            StartCoroutine(FadeOutOfExistance());
            Destroy(progressBar);
            Destroy(burnoutBar);
        }
        else if (!dead) // if not dead
        {
            float timeThing = 1f - (hydrationMeter / hydrationMeterMax);
            Color wiltColor = Color.Lerp(greenArtist.color, brownArtist.color, timeThing);
            stemRenderer.material.color = wiltColor;
            leave01.material.color = wiltColor;
            leave02.material.color = wiltColor;
            leave03.material.color = wiltColor;
            leave04.material.color = wiltColor;

            Quaternion l1 = Quaternion.Lerp(l1start.rotation, l1end.rotation, timeThing);
            Quaternion l2 = Quaternion.Lerp(l1start.rotation, l1end.rotation, timeThing);
            Quaternion l3 = Quaternion.Lerp(l1start.rotation, l1end.rotation, timeThing);
            Quaternion l4 = Quaternion.Lerp(l1start.rotation, l1end.rotation, timeThing);

            leave01.transform.rotation = l1;
            leave02.transform.rotation = l2;
            leave03.transform.rotation = l3;
            leave04.transform.rotation = l4;

        }

        watering = false;
    }

    // BURNOUT
    void ManageBurnout()
    {
        if (!dead && !watering)
        {
            burnoutMeter += Time.deltaTime * burnoutPerSecond;
            if (burnoutMeter > burnoutMeterMax)
            {
                burnoutMeter = burnoutMeterMax;
                if(!burning)
                {
                    // BURNINATING
                    // start fire vfx
                    // stop dehdration
                    // start burning coroutine
                    burningVFX.Play();
                }
            }
            bBar.UpdateProgress(burnoutMeter / burnoutMeterMax);
        }
        else if(watering)
        {
            burnoutMeter -= wateringRelief * Time.deltaTime;
            if (burnoutMeter < 0f)
                burnoutMeter = 0f;
        }

        

        progPerSecond = progPerSecStart + ((burnoutMeter / burnoutMeterMax) * burnoutProductivity);

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water") && hydrationMeter <= hydrationMeterMax)
        {
            //Debug.Log("Watering");

            watering = true;
            hydrationMeter += hydrationPerSecond * Time.deltaTime;
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

        stemRenderer.material = plantFadeOut;
        leave01.material = plantFadeOut;
        leave02.material = plantFadeOut;
        leave03.material = plantFadeOut;
        leave04.material = plantFadeOut;

        potRenderer.material = potFadeOut;

        Material tempPlantMat = stemRenderer.material;
        Material tempPotMat = potRenderer.material;

        dustPS.Play();

        while (timer <= fadeOutTime)
        {
            timer += Time.deltaTime;
            Color tempColor = Color.Lerp(tempPlantMat.color, invisible.color, (timer / fadeOutTime));
            stemRenderer.material.color = tempColor;
            leave01.material.color = tempColor;
            leave02.material.color = tempColor;
            leave03.material.color = tempColor;
            leave04.material.color = tempColor;
            potRenderer.material.color = Color.Lerp(tempPotMat.color, invisible.color, (timer / fadeOutTime));

            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(this.gameObject);
    }

}
