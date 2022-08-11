using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    [SerializeField] private GameObject m_Bar;
    [SerializeField] private GameObject m_KnobPower;
    [SerializeField] private GameObject m_KnobSteer;
    private RectTransform powerTrans;
    private RectTransform steerTrans;

    private float m_Value;
    [SerializeField] private float m_MinValue = -1.0f;
    [SerializeField] private float m_MaxValue = 1.0f;

    [SerializeField] private float m_BarSpeed = 5f;
    private bool m_DecreaseValue;
    private float m_BarWidth;

    void Awake()
    {
        powerTrans = m_KnobPower.GetComponent<RectTransform>();
        steerTrans = m_KnobSteer.GetComponent<RectTransform>();
        
        m_DecreaseValue = false;
        m_Value = -1.0f;
        m_BarWidth = m_Bar.GetComponent<RectTransform>().rect.width;
        KnobIsPower();
    }

    void Update()
    {
        switch(m_DecreaseValue)
        {
            case true:
                m_Value-= m_BarSpeed * Time.deltaTime;
                if(m_Value <= m_MinValue)
                {
                    m_DecreaseValue = false;
                }
                break;
            case false:
                m_Value+= m_BarSpeed * Time.deltaTime;
                if(m_Value >= m_MaxValue)
                {
                    m_DecreaseValue = true;
                }
                break;
        }
        if(m_KnobSteer.activeSelf == true)
        {
            steerTrans.localPosition = new Vector3(m_Value * (m_BarWidth/2), 0.0f, 0.0f);
        }
        else 
        {
            powerTrans.localPosition = new Vector3(m_Value * (m_BarWidth/2), 0.0f, 0.0f);
        }
    }

    public float GetValue()
    {
        float v = m_Value;
        return v;
    }

    public void SetValue(float value)
    {
        m_Value = Mathf.Clamp(value, m_MinValue, m_MaxValue);
    }

    public void KnobIsPower()
    {
        m_KnobPower.gameObject.SetActive(true);
        m_KnobSteer.gameObject.SetActive(false);
    }

    public void KnobIsSteer()
    {
        m_KnobSteer.gameObject.SetActive(true);
    }
}
