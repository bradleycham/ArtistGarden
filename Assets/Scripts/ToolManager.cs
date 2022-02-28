using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tools
{
    WateringCan,
    Sprinkler,
    MAX
}
public class ToolManager : MonoBehaviour
{
    public Transform handSpot;

    WateringCan can;
    Sprinkler sprinkler;

    public Transform[] tools;
    bool[] inInventory;

    Tools currentTool;

    public float throwPower = 250f;

    // Start is called before the first frame update
    void Start()
    {
        inInventory = new bool[(int)Tools.MAX];
        for(int i = 0; i < inInventory.Length; i++)
        {
            inInventory[i] = true;
        }
        can = tools[(int)Tools.WateringCan].GetComponent<WateringCan>();
        sprinkler = tools[(int)Tools.Sprinkler].GetComponent<Sprinkler>();
        sprinkler.Sprinkle(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    void IncCurrentTool(bool positive)
    {
        if(positive)
        {
            currentTool++;
            if((int)currentTool == tools.Length)
            {
                currentTool = Tools.WateringCan;
            }
        }
        else
        {
            currentTool--;
            if((int)currentTool == -1)
            {
                currentTool = Tools.Sprinkler;
            }
        }
    }
    void CheckInputs()
    {
        if (Input.mouseScrollDelta.y > 0f || Input.GetKeyDown(KeyCode.E))
        {

            IncCurrentTool(true);
            
            if (inInventory[(int)currentTool])
                ActivateTool((int)currentTool);
        }
        else if (Input.mouseScrollDelta.y < 0f || Input.GetKeyDown(KeyCode.Q))
        {
            IncCurrentTool(false);

            if(inInventory[(int)currentTool])
                ActivateTool((int)currentTool);
        }
        else
        {
            // not scrolling
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            UseTool();
        }
    }
    void UseTool()
    {
        if (currentTool == Tools.WateringCan)
        {
            can.WaterOnOff();
        }
        else if (currentTool == Tools.Sprinkler && inInventory[(int)Tools.Sprinkler])
        {
            tools[(int)Tools.Sprinkler].GetComponent<Rigidbody>().isKinematic = false;
            tools[(int)Tools.Sprinkler].parent = null;
            tools[(int)Tools.Sprinkler].GetComponent<Rigidbody>().AddForce(transform.forward * throwPower);

            sprinkler.Sprinkle(true);

            inInventory[(int)currentTool] = false; //throw away sprinkler
        }
        //else if(currentTool == Tools.Coffee)
        {

        }
        //else if(currentTool == Tools.Cookie)
        {

        }
        //else if(currentTool == Tools.Plushie)
        {

        }
    }

    void ActivateTool(int t)
    {
        for (int i = 0; i < (int)Tools.MAX; i++)
        {
            if(inInventory[i])
                tools[i].gameObject.SetActive(false);
            if (i == t)
            {
                tools[i].gameObject.SetActive(true);

                if (tools[i].gameObject.name == "Sprinkler")
                {
                    tools[i].GetComponent<Rigidbody>().isKinematic = true;
                }

                currentTool = (Tools)i;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(!inInventory[(int)Tools.Sprinkler] && other.name == "Sprinkler")
        {
            Debug.Log("InSprinkler");
            if(Input.GetKey(KeyCode.F))
            {
                tools[(int)Tools.Sprinkler] = other.transform.root;
                other.transform.parent = handSpot;
                other.transform.position = handSpot.position;
                other.transform.rotation = handSpot.rotation;
                sprinkler.Sprinkle(false);

                inInventory[(int)Tools.Sprinkler] = true;
                currentTool = Tools.WateringCan;
                ActivateTool((int)currentTool);
            }
        }
    }

    
}


