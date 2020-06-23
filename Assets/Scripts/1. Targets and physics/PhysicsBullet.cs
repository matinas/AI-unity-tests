using UnityEngine;

public class PhysicsBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.Angle(GetComponent<Rigidbody>().velocity, transform.forward), 0.0f, 0.0f);
    }
}
