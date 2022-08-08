using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    [SerializeField] private Slider m_slider;
    private bool m_DecreaseValue;
    private float m_BarSpeed = 5f;

    void Awake()
    {
        m_slider = GetComponent<Slider>();
        m_DecreaseValue = false;
        m_slider.value = -1.0f;
    }

    void Update()
    {
        switch(m_DecreaseValue)
        {
            case true:
                m_slider.value-= m_BarSpeed * Time.deltaTime;
                if(m_slider.value <= -1.0f)
                {
                    m_DecreaseValue = false;
                }
                break;
            case false:
                m_slider.value+= m_BarSpeed * Time.deltaTime;
                if(m_slider.value >= 1.0f)
                {
                    m_DecreaseValue = true;
                }
                break;
        }
    }

    public float GetValue()
    {
        float v = m_slider.value;
        return v;
    }

    public void SetValue(float value)
    {
        m_slider.value = Mathf.Clamp(value, m_slider.minValue, m_slider.maxValue);
    }
}
