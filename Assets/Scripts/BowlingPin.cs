using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPin : MonoBehaviour
{
    [Header("Parameters")]
    public bool m_beenHit;
    [SerializeField] private Vector3 m_com;

    [Header("Components")]
    [SerializeField] private AudioSource m_audio;
    [SerializeField] private Rigidbody m_rb;

    void Awake()
    {
        m_audio = GetComponent<AudioSource>();
        m_rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        m_rb.centerOfMass = m_com;
    }

    void Update()
    {
        if(!m_beenHit) return;
    }

    void PlayHitSound()
    {
        if(m_beenHit == true) return;
        m_beenHit = true;
        m_audio.Play();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("BowlingBall"))
        {
            PlayHitSound();

            float force = collision.gameObject.GetComponent<BowlingBall>().m_Power;
            Vector3 dir = (this.transform.position - collision.gameObject.transform.position).normalized;
            m_rb.AddForce(dir * force, ForceMode.Impulse);
            m_rb.centerOfMass = Vector3.zero;
        }
    }
}
