using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private float precision = 0.2f;

    void LateUpdate()
    {
        Vector3 direction = target.transform.position - gameObject.transform.position; // Direction vector towards the target
        direction.Normalize();

        if (direction.magnitude > precision) // If we are not yet closer enough, we move
        {
            Debug.DrawRay(gameObject.transform.position, direction, Color.red);
            gameObject.transform.LookAt(target.transform);                                   // Rotate the player to face the target
            gameObject.transform.Translate(direction * speed * Time.deltaTime, Space.World); // Move the player towards the target (move against direction in world space)

            // Another way of doing the same...
            // gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self); // Move the player towards the target (move forward in object space)
        }
    }
}
