using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaGoalDetection : MonoBehaviour
{
    public GamePlayController timer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            timer.PlayerReachedGoal();
        }
    }
}
