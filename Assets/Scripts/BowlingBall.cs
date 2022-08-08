using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    [Header("Parameters")]
    public float m_Power;
    public float m_Drift;
    public float m_Speed;
    public Vector3 m_MovDir;

    [Header("Physics")]
    private float m_currentHeight;
    private float m_currentSpeed;
    public float m_currentDrift;
    private float m_driftTime;
    private float m_driftDur = 2.0f;
    private bool m_canRoll;
    public GameObject m_LineOfSight;
    private bool m_HitPin;

    [Header("Components")]
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private Collider m_col;

    void Awake()
    {
        m_col = this.GetComponent<Collider>();
    }

    void Start()
    {
        m_currentHeight = transform.position.y;
        EnablePhysics(false);
        m_driftTime = 0.0f;
    }

    void Update()
    {
        // This is to prevent the bowling ball from
        // having it's Y axis go higher.
        if(m_currentHeight > transform.position.y)
        {
            m_currentHeight = transform.position.y;
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

        // Alter the movement
        if(m_MovDir != Vector3.zero)
        {
            m_MovDir = new Vector3(m_rb.velocity.x + m_currentDrift, m_rb.velocity.y, 1f * (m_Speed * m_Power));
            // Slowly rotate the bowling ball, so it starts to drift.
            if(m_driftTime < 1.0f)
            {
                m_driftTime += Time.deltaTime / (m_driftDur / m_Power);
                m_currentDrift = Mathf.Lerp(0.0f, m_Drift * 0.25f, m_driftTime);
            }
        }
    }

    void FixedUpdate()
    {
        m_rb.velocity = m_MovDir;
    }

    void EnablePhysics(bool state)
    {
        m_col.enabled = state;
        m_rb.useGravity = state;
    }

    public void RollBall(float power, float drift)
    {
        // Power affects the speed.
        // Drift causes the ball to drift slightly to the left(-1f) or right(1f).
        m_Power = (1f - Mathf.Abs(Mathf.Clamp(power, -0.9f, 1f)));
        m_Drift = Mathf.Clamp(drift, -1f, 1f);
        EnablePhysics(true);

        m_MovDir = Vector3.forward;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("BowlingPin") && m_HitPin == false)
        {
            m_HitPin = true;
            Camera.main.GetComponent<BowlingCamera>().FocusOnField();
        }
    }
}
