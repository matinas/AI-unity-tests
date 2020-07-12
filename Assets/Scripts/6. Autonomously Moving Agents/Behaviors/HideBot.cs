using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class HideBot : MonoBehaviour
{
    private NavMeshAgent _agent;

    private List<GameObject> _obstacles;

    [SerializeField]
    private Transform Player;

    [SerializeField]
    [Range(0.5f, 5.0f)]
    private float HideCloseness;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _obstacles = GameObject.FindGameObjectsWithTag("Obstacle").ToList();
    }

    private void Hide()
    {
        if (_agent.remainingDistance < 0.5f && CheckPlayerVisible())
        {
            var closestObstacle = FindClosestObstacleForward(); 
            var candidatePos = closestObstacle.transform.position;

            var playerObstacleDir = closestObstacle.transform.position - Player.transform.position;

            playerObstacleDir.Normalize();

            candidatePos += playerObstacleDir * HideCloseness; // move the candidate position a little bit in the opposite direction of the
                                                               // player wrt the obstacle so to actually get hidden by it

            // #1 way to adjust the candidatePos
            // by ensuring HideCloseness is enough to pass the back of the obstacle's BB
            // then finding the closest point on the BB (not yet completely generic as it depends on HideCloseness though!)
            var colBounds = closestObstacle.GetComponent<Collider>().bounds;
            var closestToObstacleBound = colBounds.ClosestPoint(candidatePos); // adjust the candidate position to the bounds of the obstacle so it's as close as possible

            _agent.SetDestination(closestToObstacleBound);
        }
    }

    // #2 way to adjust the hide position (completely generic and obstacle-size-agnostic)
    // by tracing a ray from the player to the obstacle, and finding the back-most intersection with the collider

    // thing is that it's not as easy as it sounds, as the Physics.Raycast will only get one hit per collider. We could get the first collider hit, and then
    // cast another ray from there to get the second one, but unfortunately "Raycasts won't detect Colliders for which the Raycast origin is inside the Collider" (from Unity doc)...
    // so we need to do all this manually...
    private void ObstacleAgnosticHide()
    {
        if (_agent.remainingDistance < 0.5f && CheckPlayerVisible())
        {
            var closestObstacle = FindClosestObstacleForward(); 
            var playerObstacleDir = closestObstacle.transform.position - Player.transform.position;
            playerObstacleDir.Normalize();

            RaycastHit[] hitInfos;
            hitInfos = Physics.RaycastAll(Player.transform.position, playerObstacleDir, 10.0f);                       // find all obstacles hits (there could be more obstacles
            foreach (var h in hitInfos)                                                                               // between the player and the selected obstacle)
            {
                if (GameObject.Equals(h.transform.gameObject, closestObstacle))                                       // select the obstacle we want from all the hits
                {
                    Debug.Log($"Had hit the selected obstacle! Hit point: {h.point}");

                    var localPoint = h.transform.InverseTransformPoint(h.point);                                      // convert hit point to obstacle's local coordinates
                    var playerObstacleDirLocal = h.transform.InverseTransformDirection(playerObstacleDir).normalized; // convert player-obstacle direction to obstacle's local coordinates
                    
                    var oppositeLocalPoint = -localPoint;                                                             // point falls right onto the collider, so the opposite point falls in the back
                    var traverseVector = oppositeLocalPoint - localPoint;                                             // horizontal vector, from the point on the front to the point on the back

                    var angle = Vector3.Angle(playerObstacleDirLocal, traverseVector.normalized) * Mathf.Deg2Rad;     // find angle between player-obstacle direction and the traverse vector
                    var distanceFromHit = traverseVector.magnitude / Mathf.Cos(angle);                                // knowing the angle and the collider's traversal size we can use trigonomery
                                                                                                                      // to calculate the diagonal distance of hit (distance from point to back hit)

                    var localBackHitPoint = localPoint + playerObstacleDirLocal * distanceFromHit;                    // find the back hit point in local space
                    var backHitPoint = h.transform.TransformPoint(localBackHitPoint);                                 // convert the back hit point back to world space

                    Debug.Log($"Back hit point: {backHitPoint}");

                    _agent.SetDestination(backHitPoint);

                    break;
                }
            }
        }
    }

    private bool CheckPlayerVisible()
    {
        Vector3 rayDir = Player.transform.position - transform.position;
        RaycastHit hitInfo;
        
        if (Physics.Raycast(transform.position, rayDir, out hitInfo))
        {
            if (hitInfo.transform.gameObject.tag == "Player")
            {
                return true;
            }
        }

        return false;
    }

    private GameObject FindClosestObstacle() // finds the closest obstacle among all the obstacles
    {
        var distance = float.MaxValue;
        GameObject closest = null;

        foreach (var o in _obstacles)
        {
            var distToObstacle = Vector3.Distance(transform.position, o.transform.position);
            if (distToObstacle < distance)
            {
                distance = distToObstacle;
                closest = o;
            }
        }

        return closest;
    }

    private GameObject FindClosestObstacleForward() // finds the closest obstacle among all the obstacles that are forward wrt the bot
    {                                               // if none is found in the forward direction it falls-back to FindClosestObstacle
        var distance = float.MaxValue;
        GameObject closest = null;

        foreach (var o in _obstacles)
        {
            var dirToObstacle = o.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, dirToObstacle) <= 90.0f)
            {
                if (dirToObstacle.magnitude < distance)
                {
                    distance = dirToObstacle.magnitude;
                    closest = o;
                }
            }
        }

        return closest ? closest : FindClosestObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        ObstacleAgnosticHide();
    }
}
