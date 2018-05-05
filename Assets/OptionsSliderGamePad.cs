using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;


public class OptionsSliderGamePad : MonoBehaviour {

    public Slider m_Slider;
    public float m_value;

    public void OnClickSlider(bool backwards)
    {
        m_value = m_Slider.value;
        if (backwards)
            m_value -= 1.0f;
        else
            m_value += 1.0f;

        m_Slider.value = m_value;
    }
}
