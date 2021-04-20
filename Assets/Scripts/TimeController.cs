using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField] public float secondsInDay = 120f;

    [Range(0, 1)] [SerializeField] public float currentTimeOfDay = 0;

    private float timeMultiplier = 1f;
    private float sunInitialIntensity;

    private GameObject[] allLights;
    private bool isLightsOn = false;

    void Start()
    {
        sunInitialIntensity = sun.intensity;

        allLights = GameObject.FindGameObjectsWithTag("LampLight");

    }


    void Update()
    {
        UpdateSun();
        UpdateLamps();

        currentTimeOfDay += (Time.deltaTime / secondsInDay) * timeMultiplier;
        if (currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
        }
    }

    void UpdateSun()
    {
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);
        float intensityMultiplier = 1;
        isLightsOn = false;

        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
        {
            intensityMultiplier = 0;
            isLightsOn = true;
        }
        else if (currentTimeOfDay <= 0.25f)
        {
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }
        else if (currentTimeOfDay >= 0.73f)
        {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
        }

        sun.intensity = sunInitialIntensity * intensityMultiplier;
    }

    void UpdateLamps()
    {
        foreach (GameObject i in allLights)
        {
            i.SetActive(isLightsOn);
        }

    }
}
