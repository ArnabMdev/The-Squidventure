using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerabound : MonoBehaviour
{
    public GameObject player;
    public float LeftLimit;
    public float RightLimit;
    public float UpLimit;
    public float BottomLimit;

    void Update()
    {
        transform.position = new Vector3
            (
                Mathf.Clamp(transform.position.x, LeftLimit, RightLimit),
                Mathf.Clamp(transform.position.y, BottomLimit, UpLimit),
                transform.position.z

            );
    }
}
