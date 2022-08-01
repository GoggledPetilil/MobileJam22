using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    [Header("Parameters")]
    public float m_Speed;

    [Header("Components")]
    [SerializeField] private Rigidbody m_rb;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        m_rb.velocity = new Vector3(m_rb.velocity.x, m_rb.velocity.y, 1.0f) * m_Speed;
    }
}
