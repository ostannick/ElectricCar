using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PressureSlider : MonoBehaviour
{
    public float Pressure
    {
        get { return SensitivityCurve.Evaluate(Slider.normalizedValue); }
    }

    Slider Slider;

    public event Action<float> BroadcastPressureChange;

    public AnimationCurve SensitivityCurve;

    // Start is called before the first frame update
    void Awake()
    {
        Slider = GetComponent<Slider>();
    }

}
