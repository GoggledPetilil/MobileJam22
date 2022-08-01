using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    [Header("Parameters")]
    public float m_Power;
    public float m_Speed;
    private Vector3 m_currentSpeed;

    [Header("Components")]
    [SerializeField] private Rigidbody m_rb;

    void Start()
    {
        RollBall();
    }

    void Update()
    {
        // This is to prevent the bowling ball from losing
        // speed when colliding against the pins.
        if(m_currentSpeed.z < m_rb.velocity.z)
        {
            m_currentSpeed = m_rb.velocity;
        }
        else if(m_currentSpeed.z > m_rb.velocity.z)
        {
            m_rb.velocity = m_currentSpeed;
        }
    }

    void RollBall()
    {
        m_rb.AddForce(Vector3.forward * m_Speed * m_Power);
    }
}
