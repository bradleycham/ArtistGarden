using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{

    public Transform waterCan;
    public Transform sprinkler;
    float scrollWheel;

    public Transform[] tools;
    int toolCount;
    // Start is called before the first frame update
    void Start()
    {
        toolCount = tools.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mouseScrollDelta.y > 0f)
        {
            scrollWheel = Input.mouseScrollDelta.y;
            Debug.Log("Scroll " + scrollWheel);
        }
        else if(Input.mouseScrollDelta.y < 0f)
        {
            scrollWheel = Input.mouseScrollDelta.y;
        }
        else
        {
            // not scrolling
            scrollWheel = 0f;
        }
        ActivateTool(2);
    }

    void ActivateTool(int t)
    {
        for(int i = 0; i < toolCount; i++)
        {
            tools[i].gameObject.SetActive(false);
            if(i == t)
            {
                tools[i].gameObject.SetActive(true);
            }
        }
    }
}


