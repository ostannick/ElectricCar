using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Engine : MonoBehaviour
{
    public Ignition Ignition;

    public AudioSource AUD;

    public float MinRPM = 0;
    public float MaxRPM = 5000;
    public float NormalizedRPM
    {
        get { return RPM / (MaxRPM - MinRPM); }
    }
    public float RPM;

    public float IdleRPM = 15;

    public TMP_Text RPMText;

    public PressureSlider GasPedal;
    public PressureSlider BrakePedal;

    public GameEvent OnThrottleDown;
    public GameEvent OnThrottleUp;

    [Range(250, 2500)]
    public float DragMultiplier = 650;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Don't do anything if the car isn't running.
        if(!Ignition.isRunning)
        {
            return;
        }

        //Engine idling always contributes a small rotational speed
        RPM = Idle(RPM);

        //Listen for throttle input
        RPM = Throttle(RPM, GasPedal.Pressure);

        //Listen for brake input
        RPM = Brake(RPM, BrakePedal.Pressure);

        //Drag (air resistance, frictional forces). RPM should naturally decay without input
        RPM = Drag(RPM);

        //Clamp value
        RPM = Mathf.Clamp(RPM, MinRPM, MaxRPM);

        //Update the UI text.
        RPMText.text = $"{RPM.ToString("00")} RPM";

        //Pitch the audio source. Currently hard-coded values.
        AUD.pitch = Mathf.Lerp(0.8f, 9f, Mathf.Pow(NormalizedRPM, 1));
        AUD.volume = Mathf.Lerp(0.65f, 1.0f, Mathf.Pow(NormalizedRPM, 1));
    }

    //Idle function.
    float Idle(float R)
    {
        R += IdleRPM * Time.deltaTime;

        return R;
    }

    //Throttle function. Can easily be adapted to a pressure-sensitive controller.
    float Throttle(float R, float Pressure)
    {
        R += Pressure * Time.deltaTime;

        return R;
    }

    //Brake function. Can easily be adapted to a pressure-sensitive controller.
    float Brake(float R, float Pressure)
    {
        R -= Pressure * Time.deltaTime;

        return R;
    }

    //Drag function
    float Drag(float R)
    {
        //You feel increased friction the faster you're moving. Linearly proportional.
        //R -= DragMultiplier * NormalizedRPM;

        //You feel increased friction the faster you're moving. Exponential. Modify the exponent as you please. Value must be clamped first to avoid NaN errors.
        RPM = Mathf.Clamp(RPM, MinRPM, MaxRPM);
        R -= DragMultiplier * Mathf.Lerp(0, 1, Mathf.Pow(NormalizedRPM, 1.25f)) * Time.deltaTime;

        return R;
    }


}
