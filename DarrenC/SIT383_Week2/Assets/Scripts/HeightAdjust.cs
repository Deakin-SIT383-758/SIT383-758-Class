using UnityEngine;

public class HeightAdjust : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveUp()
    {
        transform.position += Vector3.up * 1.0f;
    }
    
    public void MoveDown()
    {
        transform.position -= Vector3.up * 1.0f;
    }
}
