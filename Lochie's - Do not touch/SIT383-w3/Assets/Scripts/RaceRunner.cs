using UnityEngine;
using Fusion;
using TMPro;

public class RaceRunner : NetworkBehaviour
{
    public TextMeshProUGUI finish;

    //Triggers when something passes through the collider
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.tag == "Player")
        {
            finish.text = "WINNER!!";
            Debug.Log("WINNER!!");
        }
    }
}
