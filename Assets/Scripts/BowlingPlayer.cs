using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPlayer : MonoBehaviour
{
    [Header("Parameters")]
    public float m_Speed;
    public Vector3 m_MovDir;
    private bool m_Threw;

    [Header("Components")]
    [SerializeField] private BowlingBall m_BowlingBall;
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
        if(m_Threw) return;

        m_MovDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_BowlingBall.transform.parent = null;
            m_BowlingBall.RollBall(100);
            m_Threw = true;
        }
    }

    void FixedUpdate()
    {
        m_rb.velocity = m_MovDir * m_Speed;
    }
}
