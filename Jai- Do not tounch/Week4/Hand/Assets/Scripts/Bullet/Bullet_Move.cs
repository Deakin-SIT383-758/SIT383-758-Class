using UnityEngine;

public class Bullet_Move : MonoBehaviour
{
    public float speed = 1f;
    private Vector3 movementDirection;

    void Start()
    {
        // If spawned on the left side, move right (+1)
        // If spawned on the right side, move left (-1)
        if (transform.position.x > 0)
        {
            movementDirection = Vector3.right;
        }
        else
        {
            movementDirection = Vector3.left;
        }
    }

    void Update()
    {
        transform.Translate(movementDirection * speed * Time.deltaTime);
    }
}
