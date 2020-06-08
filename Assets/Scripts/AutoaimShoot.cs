using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoaimShoot : MonoBehaviour
{
    private const float Gravity = 9.8f; // This is already an acceleration, not a force, so we don't need to divide it by the object's mass

    [SerializeField]
    private Transform SpawnPoint;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private bool autoShoot;

    [SerializeField]
    private float autoShootRate;

    [SerializeField]
    private Transform BulletContainer;

    [SerializeField]
    private GameObject player;

    public Transform target;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float rotSpeed;

    private Coroutine _shootCrt;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Rotate the player towards the target
        Vector3 targetDir = target.transform.position - player.transform.position;
        
        // We could have used our AutoPilotUnity code here, but this is just another simpler way to smoothly make the rotation
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(targetDir.x, 0.0f, targetDir.z));
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, lookRotation, Time.deltaTime * rotSpeed);

        // Rotate gun towards the target based on the hit angle
        var angle = CalculateTargetAngle(true);
        if (angle != null)
        {
            // Debug.Log($"The angle is {angle}");
            transform.localEulerAngles = new Vector3(-angle.Value, 0.0f, 0.0f);
        }
        else
        {
            Debug.Log("The target can't be reached. Try increasing your projectile's speed");
        }

        if (autoShoot)
        {
            if (_shootCrt == null)
            {
                _shootCrt = StartCoroutine(ShootCoroutine());
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var go = GameObject.Instantiate(bulletPrefab, SpawnPoint.transform.position, SpawnPoint.transform.rotation, SpawnPoint);


                // To use the Unity physics engine to apply the speed that hits the target uncomment this and use the PhysicsBullet prefab
                // To use our custom velocity update implementation leave this commented, use the Bullet prefab with the ProjectileBullet enabled (won't have colissions though)

                // Shoot the bullet with the selected speed
                go.GetComponent<Rigidbody>().velocity = speed * transform.forward; // Apply the intial speed in the forward direction
                go.transform.parent = BulletContainer;
            }
        }
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            var go = GameObject.Instantiate(bulletPrefab, SpawnPoint.transform.position, SpawnPoint.transform.rotation, SpawnPoint);
            
            go.GetComponent<Rigidbody>().velocity = speed * transform.forward; // Apply the intial speed in the forward direction
            go.transform.parent = BulletContainer;

            yield return new WaitForSeconds(autoShootRate);
        }
    }

    // Calculates the angle needed to reach a certain target (x,y) given a certain speed
    // Based on "Angle theta required to hit coordinate (x,y)" from https://en.wikipedia.org/wiki/Projectile_motion
    private float? CalculateTargetAngle(bool lower)
    {
        Vector3 targetDir = target.transform.position - transform.position;

        // Calculate the actual relative (x,y) position of the target, as the original equation assumes that the projectile is thrown from (0,0) in a 2D space
        var y = targetDir.y;
        Vector3 xProj = Vector3.ProjectOnPlane(targetDir, Vector3.up); // x won't be just the magnitude of targetDir, we need to project it to y=0 plane. it'll be targetDir magnitude
        var x = xProj.magnitude;                                       // only if y is 0 as in the original equation, but our objects could be at different heights initially

        var d = Mathf.Pow(speed, 4) - Gravity*(Gravity*Mathf.Pow(x,2) + 2*y*Mathf.Pow(speed,2)); // discriminant of the quadratic equation
        if (d >= 0.0f)
        {
            var sqrtD = Mathf.Sqrt(d);
            var divisor = Gravity*x;
            var speedSqr = speed*speed;

            var root1 = (speedSqr + sqrtD) / divisor;
            var root2 = (speedSqr - sqrtD) / divisor;

            var highAngle = Mathf.Atan(root1) * Mathf.Rad2Deg;
            var lowAngle = Mathf.Atan(root2) * Mathf.Rad2Deg;

            // Debug.Log($"Higher angle is {highAngle}");
            // Debug.Log($"Lower angle is {lowAngle}");

            return (lower ? lowAngle : highAngle); // return the selected angle
        }
        else
        {
            Debug.Log("Roots for projectile motion equation are imaginary");

            return null;
        }
    }
}
