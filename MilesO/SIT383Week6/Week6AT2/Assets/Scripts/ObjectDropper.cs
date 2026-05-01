using UnityEngine;

public class ObjectDropper : MonoBehaviour
{
    public GameObject objectTemplate;
    public Vector3 startPoint = new Vector3(0, 8, 0);
    public int numberOfObjects = 40;
    public float initialSpeed = 1.0f;
    public float timeInterval = 0.3f;

    private float currentTime = 0.0f;

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > timeInterval && numberOfObjects > 0)
        {
            currentTime = 0.0f;
            numberOfObjects--;

            GameObject g = Instantiate(objectTemplate);
            g.transform.position = startPoint;

            Rigidbody rb = g.GetComponent<Rigidbody>();
            rb.linearVelocity = Random.onUnitSphere * initialSpeed;

            g.GetComponent<MeshRenderer>().material.color =
                Random.ColorHSV(0, 1, 0.5f, 1, 0.5f, 1);
        }
    }
}