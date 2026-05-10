using UnityEngine;

public class Finish : MonoBehaviour
{
    public Material finish;
    public void Done()
    {
        this.gameObject.GetComponent<Renderer>().material = finish;
    }
}
