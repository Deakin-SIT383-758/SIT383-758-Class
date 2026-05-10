using Unity.VisualScripting;
using UnityEngine;

public class Goalcube : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Finish>() != null)
        {
            other.GetComponent<Finish>().Done();
            Destroy(this.gameObject);
        }
    }
}
