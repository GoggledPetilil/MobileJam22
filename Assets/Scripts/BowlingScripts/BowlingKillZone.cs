using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingKillZone : MonoBehaviour
{
    [SerializeField] private BowlingGame game;
    [SerializeField] private GameObject m_Ball;

    void Start()
    {
        m_Ball = GameObject.FindGameObjectWithTag("BowlingBall");
    }

    void Update()
    {
        transform.position = new Vector3(m_Ball.transform.position.x, transform.position.y, m_Ball.transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BowlingBall"))
        {
            Camera.main.GetComponent<BowlingCamera>().FocusOnField();
            game.EndRound();
            //this.gameObject.SetActive(false);
            //other.gameObject.SetActive(false);
        }
    }
}
