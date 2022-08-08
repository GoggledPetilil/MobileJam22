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
    private float m_MatAlpha = 0.2f;
    public Vector3 m_startPos;

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
    [SerializeField] private Material m_Mat;

    void Awake()
    {
        m_col = this.GetComponent<Collider>();
    }

    void Start()
    {
        m_currentHeight = transform.position.y;
        EnablePhysics(false);
        m_driftTime = 0.0f;

        Color c = m_Mat.color;
        c.a = m_MatAlpha;
        m_Mat.color = c;

        m_startPos = transform.localPosition;
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
        StartCoroutine("BecomeVisible");
        EnablePhysics(true);

        m_MovDir = Vector3.forward;
    }

    public void ResetState()
    {
        EnablePhysics(false);
        m_MovDir = Vector3.zero;
        m_driftTime = 0.0f;
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero; 
        transform.rotation = Quaternion.identity;

        Color c = m_Mat.color;
        c.a = m_MatAlpha;
        m_Mat.color = c;

        m_currentHeight = 99.0f;
        transform.localPosition = m_startPos;
    }

    IEnumerator BecomeVisible()
    {
        Color c = m_Mat.color;

        float dur = 0.5f;
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime / dur;
            
            c.a = Mathf.Lerp(m_MatAlpha, 1.0f, t);
            m_Mat.color = c;

            yield return null;
        }
        yield return null;
    }
}
