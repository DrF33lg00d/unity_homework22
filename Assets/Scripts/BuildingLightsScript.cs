using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingLightsScript : MonoBehaviour
{
    private TimeController dayContoller;
    
    private bool isLightsOn;
    
    private GameObject[] lights;
    private Material[] materials;
    [SerializeField] public Material lightWindowMaterial;
    [SerializeField] public Material darkWindowMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject daySystem = GameObject.Find("DayNight");
        dayContoller = daySystem.GetComponent<TimeController>();
        isLightsOn = false;
        lights = GameObject.FindGameObjectsWithTag("Lights");
        materials = GetComponent<Renderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDay();
        UpdateLights();
    }

    void CheckDay()
    {
        if (dayContoller.currentTimeOfDay < 0.26f || dayContoller.currentTimeOfDay > 0.74f)
        {
            isLightsOn = true;
        }
        else
        {
            isLightsOn = false;
        }
    }
    
    void UpdateLights()
    {
        foreach (GameObject i in lights)
        {
            i.SetActive(isLightsOn);
        }
        if (isLightsOn)
        {
            materials[2] = lightWindowMaterial;
        }
        else
        {
            materials[2] = darkWindowMaterial;
        }
        GetComponent<Renderer>().materials = materials;
    }
}
