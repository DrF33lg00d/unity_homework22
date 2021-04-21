using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLightsScript : MonoBehaviour
{
    private TimeController dayContoller;

    private GameObject[] carLights;

    private bool isLightsOn;
    private Material[] materials;
    private Material lights_matererial;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject daySystem = GameObject.Find("DayNight");
        dayContoller = daySystem.GetComponent<TimeController>();
        carLights = GameObject.FindGameObjectsWithTag("CarLights");
        isLightsOn = false;
        materials = GetComponent<Renderer>().materials;
        lights_matererial = materials[1];
    }

    // Update is called once per frame
    void Update()
    {
        CheckDay();
        UpdateLights();
    }

    void CheckDay()
    {
        isLightsOn = dayContoller.currentTimeOfDay < 0.3f || dayContoller.currentTimeOfDay > 0.7f;
    }
    
    void UpdateLights()
    {
        foreach (GameObject i in carLights)
        {
            i.SetActive(isLightsOn);
        }

        if (isLightsOn)
        {
            materials[1] = lights_matererial;
        }
        else
        {
            materials[1] = materials[0];
        }
        GetComponent<Renderer>().materials = materials;
    }
}
