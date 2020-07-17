using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockGroup FlockGroup { get; set; }

    private Vector3 _currentDir;

    private bool _isTurning = false;

    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _currentDir = transform.forward;
        _speed = Random.Range(FlockGroup.MinSpeed, FlockGroup.MaxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (FlockGroup.RespectLimits && AboutToReachLimits())
        {
            transform.rotation = Quaternion.LookRotation(-transform.forward);
        }
        else
        {
            var newHeading = FlockGroup.ApplyFlockingRules(transform.gameObject);
            if (newHeading != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newHeading), Time.deltaTime * FlockGroup.RotationSpeed);
            }
        }

        transform.Translate(0.0f, 0.0f, Time.deltaTime * _speed);
    }

    private bool AboutToReachLimits()
    {
        return transform.position.x <= FlockGroup.MinLimit + FlockGroup.LimitAccuracy && Vector3.Dot(transform.forward, Vector3.right) < 0 ||
               transform.position.x >= FlockGroup.MaxLimit - FlockGroup.LimitAccuracy && Vector3.Dot(transform.forward, Vector3.right) > 0 ||
               transform.position.y <= FlockGroup.MinLimit + FlockGroup.LimitAccuracy && Vector3.Dot(transform.forward, Vector3.up) < 0 ||
               transform.position.y >= FlockGroup.MaxLimit - FlockGroup.LimitAccuracy && Vector3.Dot(transform.forward, Vector3.up) > 0 ||
               transform.position.z <= FlockGroup.MinLimit + FlockGroup.LimitAccuracy && Vector3.Dot(transform.forward, Vector3.forward) < 0 ||
               transform.position.z >= FlockGroup.MaxLimit - FlockGroup.LimitAccuracy && Vector3.Dot(transform.forward, Vector3.forward) > 0;
    }
}
