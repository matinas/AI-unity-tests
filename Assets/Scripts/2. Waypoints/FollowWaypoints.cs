using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowWaypoints : MonoBehaviour
{
    [SerializeField]
    private List<Transform> Waypoints;

    [SerializeField]
    private float Speed;

    private int _currentWpIndex;
    private float _accuracy = 0.5f;
    private float _rotAccuracy = 0.5f;
    private bool startTraversal = false;

    private float _rotSnapness = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        _currentWpIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startTraversal = true;
        }
    }

    void LateUpdate()
    {
        if (startTraversal)
        {
            var currentTarget = Waypoints[_currentWpIndex];
            var targetDir = currentTarget.position - transform.position;

            // look towards the target (rotate only around the Y axis)
            var angle = Vector3.SignedAngle(transform.forward, targetDir, transform.up);
            if (Math.Abs(angle) > _rotAccuracy) // avoid rotation if the angle is not big enough (smoother movement)
            {
                transform.Rotate(0.0f, angle * _rotSnapness, 0.0f, Space.Self);
            }

            // another simpler way of turning smoothly towards the target which has almost the same result
            // var rotVector = Vector3.Slerp(transform.forward, new Vector3(targetDir.x, 0.0f, targetDir.z), 0.1f) * Time.deltaTime;
            // transform.rotation = Quaternion.LookRotation(rotVector, transform.up);

            // move towards the target
            if (targetDir.magnitude > _accuracy)
            {
                transform.Translate(transform.forward * Time.deltaTime * Speed, Space.World);
            }
            else
            {
                _currentWpIndex++; // increment the current waypoint index, so to move to the next one

                if (_currentWpIndex >= Waypoints.Count)
                {
                    _currentWpIndex = 0;
                    startTraversal = false;
                }
            }
        }
    }
}
