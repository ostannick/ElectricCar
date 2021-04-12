using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignition : MonoBehaviour
{
    public Engine Engine;

    public bool isRunning = false;

    public AudioSource AUD;

    public AudioClip EnginePowerOn;
    public AudioClip EnginePowerOff;

    public AnimationCurve InitialRevCurve;

    public void Toggle()
    {
        isRunning = !isRunning;
        
        if(isRunning)
        {
            AUD.PlayOneShot(EnginePowerOn);
            Engine.AUD.Play();
            StartCoroutine(StartEngine());
        }
        else
        {
            AUD.PlayOneShot(EnginePowerOff);
        }
    }

    IEnumerator StartEngine()
    {
        float f = 0;
        while(f < InitialRevCurve.keys[InitialRevCurve.keys.Length - 1].time)
        {
            Engine.RPM += InitialRevCurve.Evaluate(f) * Time.deltaTime;

            f += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
