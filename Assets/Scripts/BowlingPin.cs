using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPin : MonoBehaviour
{
    [SerializeField] private AudioSource m_audio;
    private bool m_beenHit;

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
        }
    }
}
