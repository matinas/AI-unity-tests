using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowWaypointsWithTracker : MonoBehaviour
{
    [SerializeField]
    private List<Transform> Waypoints;

    [SerializeField]
    private float Speed;

    private GameObject _tracker;

    private int _currentWpIndex;
    private float _accuracy = 0.5f;
    private bool startTraversal = false;

    private float _rotSnapness = 0.05f;

    private float _trackerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _currentWpIndex = 0;

        _tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        _tracker.transform.position = transform.position;
        _tracker.transform.rotation = transform.rotation;
        _tracker.transform.localScale *= 0.25f;
        DestroyImmediate(_tracker.GetComponent<Rigidbody>()); // we don't need our tracker to react to physics

        _trackerSpeed = Speed * 1.1f; // we need the progress tracker to always go ahead our object
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startTraversal = true;
        }
    }

    // There's a little problem with the FollowWaypoints, which may occur for certain Speed and _ratSnapness combinations, which
    // makes that our object keeps rotating in circles infinitely around a waypoint. The problem is due to the rotation angle being
    // not enough to counter the speed of movement/translation. So if we rotate just very little but advance towards so much, we are
    // not getting closer to the waypoint but actually getting far from it on each frame. The next frame our object will try to get closer
    // again, and it will be kept in this try-to-get-closer/actually-get-farther loop forever

    // To counter this issue we need a way to make our object follow the waypoints path even when it doesn't reach a specific waypoint
    // There are many different ways of solving this. One of them is using a Progress Tracker, which follows the path exactly (no smooth
    // turning and physics needed, so we avoid the aforementioned issue), and get our object to follow the tracker instead of each waypoint

    void ProgressTrackerUpdate()
    {
        if (_currentWpIndex < Waypoints.Count)
        {
            var currentTarget = Waypoints[_currentWpIndex];
            var targetDir = currentTarget.position - _tracker.transform.position;

            // look towards the target (rotate only around the Y axis)
            var angle = Vector3.SignedAngle(_tracker.transform.forward, targetDir, _tracker.transform.up);
            _tracker.transform.Rotate(0.0f, angle, 0.0f, Space.Self); // note that we don't need _rotSnapness now here, as we don't care the tracker rotates smoothly

            // move the tracker towards the target
            if (targetDir.magnitude > _accuracy)
            {
                _tracker.transform.Translate(_tracker.transform.forward * Time.deltaTime * _trackerSpeed, Space.World);
            }
            else
            {
                _currentWpIndex++; // increment the current waypoint index, so to move to the next one
            }
        }

        // else, the progress tracker already traversed the complete path and we are just waiting that our object catchs up with the tracker
    }

    void LateUpdate()
    {
        if (startTraversal)
        {
            ProgressTrackerUpdate();

            var targetDir = _tracker.transform.position - transform.position;

            // look towards the tracker (rotate only around the Y axis)
            var angle = Vector3.SignedAngle(transform.forward, targetDir, transform.up);
            transform.Rotate(0.0f, angle * _rotSnapness, 0.0f, Space.Self);

            // move the object towards the tracker
            if (targetDir.magnitude > _accuracy)
            {
                transform.Translate(transform.forward * Time.deltaTime * Speed, Space.World);
            }
            else // when the object catchs up with the tracker we reset the waypoint traversal parameters
            {
                if (_currentWpIndex >= Waypoints.Count)
                {
                    _currentWpIndex = 0;
                    startTraversal = false;
                }
            }
        }
    }
}
