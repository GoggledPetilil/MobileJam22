using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingCamera : MonoBehaviour
{
    public GameObject m_TargetObj;
    public GameObject m_PinMiddle;
    public float m_Speed;
    public Vector3 m_Offset;

    void Start()
    {
        FocusOnPlayer();
    }

    void LateUpdate()
    {
        Vector3 endPos = m_TargetObj.transform.position + m_Offset;
        Vector3 smoothPos = Vector3.Lerp(this.transform.position, endPos, m_Speed * Time.deltaTime);
        this.transform.position = smoothPos;
    }

    public void FocusOnPlayer()
    {
        m_TargetObj = GameObject.FindGameObjectWithTag("BowlingBall");
        m_Offset = new Vector3(0.0f, 1.0f, -14.0f);
    }

    public void FocusOnField()
    {
        m_TargetObj = m_PinMiddle;
        m_Offset = new Vector3(0.0f, 10.0f, -36.0f);
        // Turn on rotation
    }
}
