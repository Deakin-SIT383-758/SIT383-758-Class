using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform camTrans;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camTrans = Camera.main.transform; // get transform of main camera in scene
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = camTrans.rotation; // match camera rotation
    }
}
