using UnityEngine;

public class FloatObject : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float floatHeight = 0.03f;

    private Vector3 startLocalPos;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = startLocalPos +
            Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatHeight;
    }
}