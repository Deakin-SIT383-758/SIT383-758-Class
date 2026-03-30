using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class DrawScript : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    private Vector3 startPos;
    private Vector3 endPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartLine(Vector3 pos)
    {
        startPos = pos;
    }

    public void EndLine(Vector3 pos)
    {
        endPos = pos;
        GameObject line = GameObject.Instantiate(linePrefab, startPos, Quaternion.identity);
        line.GetComponent<LineRenderer>().SetPosition(1, endPos);
        endPos = Vector3.zero;
        startPos = Vector3.zero;
    }
}
