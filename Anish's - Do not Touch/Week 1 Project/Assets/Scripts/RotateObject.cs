using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(0.5f, newVector3(0.3f, -0.5f, 0.1f));
    }
}