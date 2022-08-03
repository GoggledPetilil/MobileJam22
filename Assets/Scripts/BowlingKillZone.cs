using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingKillZone : MonoBehaviour
{
    [SerializeField] private BowlingGame game;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BowlingBall"))
        {
            game.KillDeadPins();
        }
    }
}
