using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardRail : MonoBehaviour
{
    public Material m_Mat;

    void Start()
    {
        Color c = m_Mat.color;
        c.a = 0.0f;
        m_Mat.color = c;
    }

    void OnCollisionEnter(Collision collision)
    {
        StopAllCoroutines();
        StartCoroutine("Flash");
    }

    IEnumerator Flash()
    {
        float flashSpeed = 2.0f;
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime * flashSpeed;

            Color c = m_Mat.color;
            c.a = Mathf.Lerp(1.0f, 0.0f, t);
            m_Mat.color = c;

            yield return null;
        }
        yield return null;
    }
}
