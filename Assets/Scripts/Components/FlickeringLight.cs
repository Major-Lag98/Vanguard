using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[ExecuteAlways]
public class FlickeringLight : MonoBehaviour
{
    [Range(0,1)]
    public float MinIntensity;

    [Range(0,1)]
    public float MaxIntensity = 1;

    [Range(0,1)]
    public float TimeBetweenFlicker = 0.3f;

    [Range(0,1)]
    public float RandomFlickerOffset = 0.1f;

    private Light2D light;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            var random = Random.Range(MinIntensity, MaxIntensity);
            light.intensity = random;
            yield return new WaitForSeconds(Random.Range(0f, TimeBetweenFlicker));
        }
    }
}
