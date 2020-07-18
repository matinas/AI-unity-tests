using System.Linq;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockGroup FlockGroup { get; set; }

    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        Speed = Random.Range(FlockGroup.MinSpeed, FlockGroup.MaxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 flockAvgPos = FlockGroup.AveragePosition;

        if (FlockGroup.RespectLimits && AboutToReachLimits())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(flockAvgPos - transform.position), Time.deltaTime * FlockGroup.RotationSpeed);
        }
        else
        {
            var newHeading = FlockGroup.ApplyFlockingRules(transform.gameObject);
            if (newHeading != Vector3.zero)
            {
                if (FlockGroup.AvoidObstacles)
                {
                    ObstacleAdjustment(ref newHeading);
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newHeading), Time.deltaTime * FlockGroup.RotationSpeed);
            }
        }

        transform.Translate(0.0f, 0.0f, Time.deltaTime * Speed);
        
        if (Vector3.Distance(transform.position, flockAvgPos) >= FlockGroup.ReflockDistance) // if a flock has gone too far from the main flock it will be returned (kinda flock pooling)
        {
            transform.position = flockAvgPos;
        }
    }
    
    private void ObstacleAdjustment(ref Vector3 heading) // adjusts the input direction so to avoid the obstacles for the flock group
    {
        // here we are simply adjusting the direction by a configurable angle (should be [90,180] so to point in the obstacle's opposite direction)
        // RaycastHit hitInfo;
        // if (Physics.Raycast(transform.position, heading, out hitInfo, FlockGroup.AvoidDistance)) // if the new direction the flock will be heading intersects a close enough object...
        // {                                   
        //     if (FlockGroup.Obstacles.Any(x => GameObject.ReferenceEquals(x.gameObject, hitInfo.transform.gameObject))) // ...and it's an obstacle, then move away
        //     {
        //         Quaternion avoidRot = Quaternion.Euler(0.0f, FlockGroup.AvoidFactor, 0.0f);
        //         heading = avoidRot * heading; // rotate heading direction away from obstacle
        //     }
        // }

        // a better more generic way of doing it is by moving towards the reflected direction of the input wrt the normal at the hit point
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, heading, out hitInfo, FlockGroup.AvoidDistance)) // if the new direction the flock will be heading intersects a close enough object...
        {                                   
            if (FlockGroup.Obstacles.Any(x => GameObject.ReferenceEquals(x.gameObject, hitInfo.transform.gameObject))) // ...and it's an obstacle, then move away
            {
                heading = Vector3.Reflect(heading, hitInfo.normal); 
            }
        }
    }

    private bool AboutToReachLimits() // we could have implemented this by creating a UnityEngine.Bounds to compare the position against
    {
        return transform.position.x <= FlockGroup.MinLimit + FlockGroup.LimitAccuracy && Vector3.Dot(transform.forward, Vector3.right) < 0 ||
               transform.position.x >= FlockGroup.MaxLimit - FlockGroup.LimitAccuracy && Vector3.Dot(transform.forward, Vector3.right) > 0 ||
               transform.position.y <= FlockGroup.MinLimit + FlockGroup.LimitAccuracy && Vector3.Dot(transform.forward, Vector3.up) < 0 ||
               transform.position.y >= FlockGroup.MaxLimit - FlockGroup.LimitAccuracy && Vector3.Dot(transform.forward, Vector3.up) > 0 ||
               transform.position.z <= FlockGroup.MinLimit + FlockGroup.LimitAccuracy && Vector3.Dot(transform.forward, Vector3.forward) < 0 ||
               transform.position.z >= FlockGroup.MaxLimit - FlockGroup.LimitAccuracy && Vector3.Dot(transform.forward, Vector3.forward) > 0;
    }
}