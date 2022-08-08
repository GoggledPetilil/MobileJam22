using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPin : MonoBehaviour
{
    [Header("Parameters")]
    public bool m_beenHit;
    public Vector3 m_com;
    [SerializeField] private LayerMask m_GroundLayer;

    [Header("Components")]
    [SerializeField] private AudioSource m_audio;
    [SerializeField] private AudioClip[] m_audioClips;
    [SerializeField] private Rigidbody m_rb;

    void Awake()
    {
        m_audio = GetComponent<AudioSource>();
        m_rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        m_rb.centerOfMass = m_com;
        m_audio.clip = m_audioClips[Random.Range(0, m_audioClips.Length)];
    }

    void Update()
    {
        float rotX = Mathf.InverseLerp(0.0f, 180.0f, transform.rotation.eulerAngles.x);
        float rotZ = Mathf.InverseLerp(0.0f, 180.0f, transform.rotation.eulerAngles.z);

        m_com.y = Mathf.Max(rotX, rotZ);
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

        float threshold = 45f;

        if(rot.eulerAngles.x >= threshold || rot.eulerAngles.x <= -threshold || rot.eulerAngles.z >= threshold || rot.eulerAngles.z <= -threshold)
        {
            isKnockedOver = true;
        }

        return isKnockedOver;
    }

    public bool isGrounded()
    {
        float distance = 1f;
        return Physics.Raycast(transform.position, -Vector3.up, distance, m_GroundLayer);
    }

    public void DestroySelf()
    {
        StartCoroutine("DestroyAnimation");
    }

    public void FreezeBody()
    {
        m_rb.constraints = RigidbodyConstraints.FreezeRotationX;
        m_rb.constraints = RigidbodyConstraints.FreezeRotationZ;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("BowlingBall"))
        {
            PlayHitSound();

            float force;
            force = collision.gameObject.GetComponent<BowlingBall>().m_Power * 100;
            
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
