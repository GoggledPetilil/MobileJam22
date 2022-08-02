using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    [Header("Parameters")]
    public float m_Power;
    public float m_Speed;
    private float m_currentHeight;
    private float m_currentSpeed;

    [Header("Components")]
    [SerializeField] private Rigidbody m_rb;

    void Start()
    {
        
        RollBall();
    }

    void Update()
    {
        // This is to prevent the bowling ball from
        // having it's Y axis go higher.
        if(m_currentHeight > transform.position.y)
        {
            m_currentSpeed = transform.position.y;
        }
        else if(m_currentHeight < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, m_currentHeight, transform.position.z);
        }
        
        // This is to prevent the bowling ball from losing
        // speed when colliding against the pins.
        if(m_currentSpeed < m_rb.velocity.z)
        {
            m_currentSpeed = m_rb.velocity.z;
        }
        else if(m_currentSpeed > m_rb.velocity.z)
        {
            m_rb.velocity = new Vector3(m_rb.velocity.x, m_rb.velocity.y, m_currentSpeed);
        }
    }

    void RollBall()
    {
        m_currentHeight = transform.position.y;
        m_rb.AddForce(Vector3.forward * m_Speed * m_Power);
    }
}
