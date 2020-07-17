using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlockGroup : MonoBehaviour
{
    [SerializeField]
    private Transform Prefab;

    [SerializeField]
    private int Count;

    [Range(-7, 7)]
    public float MinLimit;

    [Range(-7, 7)]
    public float MaxLimit;

    [Range(0.0f, 0.5f)]
    public float LimitAccuracy;

    [Header("Flock Group Settings")]
    [Range(0.1f, 5.0f)]
    public float MinSpeed;

    [Range(0.1f, 5.0f)]
    public float MaxSpeed;

    [Range(1.0f, 3.0f)]
    public float RotationSpeed;

    [Range(1.0f, 10.0f)]
    public float NeighbourDistance;

    [Range(0.5f, 5.0f)]
    public float AvoidNeighbourDistance;

    public bool RespectLimits;

    public bool MoveTowardGoal;

    public Transform GoalPosition;

     [Range(0.5f, 3.0f)]
    public float SpeedToGoal;

    private List<GameObject> _flockObjs = new List<GameObject>();
    private int FlockCount { get { return _flockObjs.Count; } }

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i < Count; ++i)
        {
            var flockPos = new Vector3(Random.Range(MinLimit, MaxLimit), Random.Range(MinLimit, MaxLimit), Random.Range(MinLimit, MaxLimit));
            var flockObj = GameObject.Instantiate(Prefab, flockPos, Prefab.rotation);
            flockObj.GetComponent<Flock>().FlockGroup = this;

            _flockObjs.Add(flockObj.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 ApplyFlockingRules(GameObject go)
    {
        var neighbourhood = _flockObjs.Where(x => !GameObject.ReferenceEquals(x, go) && (Vector3.Distance(x.transform.position, go.transform.position) <= NeighbourDistance));

        var newHeading = Vector3.zero;
        if (neighbourhood.Count() > 0)
        {
            if (MoveTowardGoal)
            {
                var flockAvgPos = FlockAveragePosition(neighbourhood);
                Vector3 shiftedAvgPos = flockAvgPos;

                if (Vector3.Distance(flockAvgPos, GoalPosition.position) > 1.0f) // shift position only if the flock group is not yet close enough to the goal position
                {
                    var avgPosToGoalDir = GoalPosition.position - flockAvgPos;
                    shiftedAvgPos = flockAvgPos + avgPosToGoalDir.normalized * SpeedToGoal;
                }

                newHeading = (shiftedAvgPos - go.transform.position) + AvoidHeadingDirection(go, neighbourhood);
            }
            else
            {
                newHeading = (FlockAveragePosition(neighbourhood) - go.transform.position) + AvoidHeadingDirection(go, neighbourhood);
            }

            // the rule "move towards the average group heading position" doesn't working that fine, and i think it makes sense
            // what happens is that all the objects in the flock end up being aligned to certain position
            // seems to be nice for certain scenarios but in a more general scenario it's better to have a bit more noise/randomness in the behavior of each individual
            // newHeading = FlockAverageHeading(neighbourhood) + (FlockAveragePosition(neighbourhood) - go.transform.position) + AvoidHeadingDirection(go, neighbourhood);
        }

        return newHeading;
    }

    // we are not using this but we would want to take into account the global speed of the flock to set the speed of each flock (set the speed of
    // each flock to this as part of the flocking rules application). right now even when an individual is part of a flock group it moves at its own
    // speed randomly calculated at startup this means that when we have the MoveTowardGoal flag enabled some of the individuals will move slower than
    // others towards the target
    public float GetFlockGlobalSpeed(IEnumerable<GameObject> neighbourhood)
    {
        var gSpeed = 0.0f;
        foreach (var neighbour in neighbourhood)
        {
            gSpeed += neighbour.GetComponent<Flock>().Speed;
        }

        return gSpeed / neighbourhood.Count();
    }

    private Vector3 FlockAverageHeading(IEnumerable<GameObject> neighbourhood)
    {
        Vector3 avgHeading = Vector3.zero;
        
        foreach (var neighbour in neighbourhood)
        {
            avgHeading += neighbour.transform.forward;
        }
        avgHeading.Normalize();

        return avgHeading;
    }

    private Vector3 FlockAveragePosition(IEnumerable<GameObject> neighbourhood)
    {
        Vector3 avgPosition = Vector3.zero;
        foreach (var neighbour in neighbourhood)
        {
            avgPosition += neighbour.transform.position;
        }
        avgPosition /= neighbourhood.Count();

        return avgPosition;
    }

    private Vector3 AvoidHeadingDirection(GameObject go, IEnumerable<GameObject> neighbourhood)
    {
        var avoidHeadingDir = Vector3.zero;
        foreach (var neighbour in neighbourhood)
        {
            if (Vector3.Distance(neighbour.transform.position, go.transform.position) < AvoidNeighbourDistance)
            {
                var neighbourAvoidDir = go.transform.position - neighbour.transform.position; // opposite direction to the neightbour-to-go direction

                avoidHeadingDir += neighbourAvoidDir;
            }
        }
        // avoidHeadingDir.Normalize();

        return avoidHeadingDir;
    }

    private GameObject ClosestNeighbour(GameObject obj)
    {
        GameObject closest = null;
        float closestDist = float.MaxValue;

        foreach (var o in _flockObjs.Where(x => !GameObject.ReferenceEquals(obj, x)))
        {
            var dist = Vector3.Distance(o.transform.position, obj.transform.position);
            if (dist < closestDist)
            {
                closest = o;
                closestDist = dist;
            }
        }

        return closest;
    }
}
