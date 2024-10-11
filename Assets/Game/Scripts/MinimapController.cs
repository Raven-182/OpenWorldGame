using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public Transform player; 
    public float height = 25f;  

    void Update()
    {  
        Vector3 newPosition = player.position;
        newPosition.y += height;  
        transform.position = newPosition;
    }
}
