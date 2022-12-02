using System.Collections;
using UnityEngine;

public class PulsatingLight : MonoBehaviour
{
    private Light _light;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private float minIntensity = 0f;
    [SerializeField] private float pulseSpeed = 5f; //here, a value of 0.5f would take 2 seconds and a value of 2f would take half a second

    private float targetIntensity = 1f;
    private float currentIntensity;


    void Start()
    {
        _light = GetComponent<Light>();
    }
    void Update()
    {
        currentIntensity = Mathf.MoveTowards(_light.intensity, targetIntensity, Time.deltaTime * pulseSpeed);
        if (currentIntensity >= maxIntensity)
        {
            currentIntensity = maxIntensity;
            targetIntensity = minIntensity;
        }
        else if (currentIntensity <= minIntensity)
        {
            currentIntensity = minIntensity;
            targetIntensity = maxIntensity;
        }
        _light.intensity = currentIntensity;
    }

    public void HandleDamage()
    {
        StartCoroutine(FastFlash());
    }

    private IEnumerator FastFlash()
    {
        float currentSpeed = pulseSpeed;
        pulseSpeed *= 2;
        yield return new WaitForSeconds(2f);
        pulseSpeed = currentSpeed;
    }
}