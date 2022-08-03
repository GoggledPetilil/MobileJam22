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

    void PlayHitSound()
    {
        if(m_beenHit == true) return;
        m_beenHit = true;
        m_audio.Play();
    }

    public bool isKnockedOver()
    {
        bool isKnockedOver = false;

        Quaternion rot = transform.rotation;

        float threshold = 64f;

        if(rot.eulerAngles.x >= threshold || rot.eulerAngles.x <= -threshold || rot.eulerAngles.z >= threshold || rot.eulerAngles.z <= -threshold)
        {
            isKnockedOver = true;
        }

        return isKnockedOver;
    }

    public void DestroySelf()
    {
        StartCoroutine("DestroyAnimation");
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("BowlingBall"))
        {
            PlayHitSound();

            float force;
            force = collision.gameObject.GetComponent<BowlingBall>().m_Power;
            
            Vector3 dir = (this.transform.position - collision.gameObject.transform.position).normalized;
            m_rb.AddForce(dir * force, ForceMode.Impulse);
            m_rb.centerOfMass = Vector3.zero;
        }
    }

    IEnumerator DestroyAnimation()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float scaleDur = 1f;
        float t = 0.0f;
        while(t < 1.0f)
        {
            t += Time.deltaTime / scaleDur;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
