using UnityEngine;

public class ActiveTimer : MonoBehaviour
{
    public GameObject display;
    float timer = 0;
    public float interval;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0;
            display.SetActive(false);
        }
    }
}
