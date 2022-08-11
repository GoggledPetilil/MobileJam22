using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    [Header("Parameters")]
    public float m_Speed;
    public Vector2 m_MovDir;
    [Header("Components")]
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private Joystick m_joystick;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(GameManager.instance.isDesktop)
        {
            m_MovDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }
        else
        {
            m_MovDir = new Vector2(m_joystick.Horizontal, m_joystick.Vertical);
        }
    }

    void FixedUpdate()
    {
        Vector3 mov = new Vector3(m_MovDir.x, m_rb.velocity.y, m_MovDir.y);
        m_rb.velocity = mov * m_Speed;
    }
}
