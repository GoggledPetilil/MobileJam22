using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingMiddle : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BowlingBall"))
        {
            Camera.main.GetComponent<BowlingCamera>().FocusOnField();
        }
    }
}
