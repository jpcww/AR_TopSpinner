using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class ScaleControl : MonoBehaviour
{
    ARSessionOrigin m_ARSessionOrigin;
    public Slider scaleSlider;

    public void Awake()
    {
        m_ARSessionOrigin = GetComponent<ARSessionOrigin>();
    }


    // Start is called before the first frame update
    void Start()
    {
        scaleSlider.onValueChanged.AddListener(OnSlidervalueChanged);       // maybe the slider would generate its value, the parameter for this method is not necessary
    }

    public void OnSlidervalueChanged(float value)
    {
        if(scaleSlider != null)
        {
            m_ARSessionOrigin.transform.localScale = Vector3.one * value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
