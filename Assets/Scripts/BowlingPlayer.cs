using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPlayer : MonoBehaviour
{
    [Header("Parameters")]
    public float m_Speed;
    public Vector3 m_MovDir;
    private bool m_Threw;       // This just tells wether the bowling ball has been thrown.
    private int m_throwStage;   // What the player is currently doing with the ball(lining up, charging power, etc.)
    public float m_BallPower;   // The power the Player has charged.
    public float m_BallDrift;   // The drift the Player is suffering.
    private float m_StartZ;
    private float m_EndZ;
    private float m_PosTimer;
    public float m_ThrowTime;   // How long the player has to throw the ball.

    [Header("Components")]
    [SerializeField] private BowlingBall m_BowlingBall;
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private PowerBar m_powerBar;

    [Header("Audio")]
    [SerializeField] private AudioSource m_as;
    [SerializeField] private AudioClip m_SFXPower;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_as = GetComponent<AudioSource>();
        m_throwStage = 0;
    }

    void Start()
    {
        m_powerBar.gameObject.SetActive(false);

        m_EndZ = transform.position.z;
        m_StartZ = m_EndZ - 5f;
        transform.position = new Vector3(transform.position.x, transform.position.y, m_StartZ);
        Invoke("TurnOnInterpolate", 0.1f); // Turning on Interpolate on the same frame cancels transform.position, for some dumb reason
    }

    void Update()
    {
        if(m_Threw) return;

        m_MovDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            switch(m_throwStage)
            {
                case 0:
                    // The first click will active the power bar, letting the player get ready.
                    m_rb.velocity = Vector3.zero;

                    m_powerBar.gameObject.SetActive(true);
                    m_powerBar.KnobIsPower();
                    m_powerBar.SetValue(-1f);
                    
                    m_BowlingBall.m_LineOfSight.SetActive(false);
                    break;
                case 1:
                    // The second click will get the power.
                    // The Player wants to get the power in the center.
                    m_BallPower = m_powerBar.GetValue();
                    PlaySound(m_SFXPower);

                    m_powerBar.SetValue(-1f);
                    m_powerBar.KnobIsSteer();
                    break;
                case 2:
                    // Drift needs to either drift to the left(-1) or right(1).
                    // Being more in the center lessens the drift.
                    m_BallDrift = m_powerBar.GetValue();
                    PlaySound(m_SFXPower);
                    
                    // The bowling ball is now thrown using the values the Player got.
                    ThrowBall();

                    m_powerBar.gameObject.SetActive(false);
                    break;
            }
            m_throwStage++;
        }

        if(m_throwStage > 0 && m_throwStage < 3)
        {
            if(m_PosTimer < 1.0f)
            {
                m_PosTimer += Time.deltaTime / m_ThrowTime;
                float newZ = Mathf.Lerp(m_StartZ, m_EndZ, m_PosTimer);
                transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
            }
            else 
            {
                // The player has ran out of time, so just use what's been given.
                if(m_throwStage < 2)
                {
                    m_BallPower = Random.Range(0.9f, 1.0f);
                }
                if(m_throwStage < 3)
                {
                    m_BallDrift = m_powerBar.GetValue();
                }

                ThrowBall();
                m_powerBar.gameObject.SetActive(false);
                m_throwStage = 3;
            }
        }
    }

    void FixedUpdate()
    {
        if(m_throwStage <= 0)
        {
            m_rb.velocity = m_MovDir * m_Speed;
            m_BowlingBall.transform.position = new Vector3(transform.position.x + 1.5f, m_BowlingBall.transform.position.y, m_BowlingBall.transform.position.z);
        }
    }

    void ThrowBall()
    {
        m_BowlingBall.transform.parent = null;
        m_BowlingBall.RollBall(m_BallPower, m_BallDrift);
        m_Threw = true;
    }

    void TurnOnInterpolate()
    {
        // god dammit unity you piece of shit software
        m_rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void PlaySound(AudioClip clip)
    {
        m_as.clip = clip;
        m_as.Play();
    }

    public void ResetState()
    {
        m_throwStage = 0;
        m_Threw = false;
        m_rb.velocity = Vector3.zero;
        m_BowlingBall.m_LineOfSight.SetActive(true);

        this.transform.position = new Vector3(0.0f, transform.position.y, m_StartZ);
        m_BowlingBall.transform.localPosition = new Vector3(transform.position.x + 1.5f, 0.2f, 1.0f);
    }
}
