using UnityEngine;
using AITests.AMA.States;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

public class BotController : MonoBehaviour
{
    public event Action OnHiddenFromPlayer;
    public event Action OnKeepHiddenCompleted;
    public event Action OnWanderCompleted;

    [SerializeField]
    private Transform Player;

    [SerializeField]
    [Range(0.1f, 10.0f)]
    private float WanderRange;

    private State _state;

    private NavMeshAgent _agent;

    private List<GameObject> _obstacles;

    private bool _justHidden = false;
    private bool _wandering = false;

    // Start is called before the first frame update
    void Start()
    {
        _state = new SeekState(gameObject.transform);
        _state.OnStateChangedEvent += HandleStateChangedEvent;

        _obstacles = GameObject.FindGameObjectsWithTag("Obstacle").ToList();
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _state.Update();
    }

    private void HandleStateChangedEvent(StateChangedEvt e)
    {
        _state.OnStateChangedEvent -= HandleStateChangedEvent;

        switch (e.newState)
        {
            case States.SEEK:
            {
                _state = new SeekState(gameObject.transform);
                break;
            }
            case States.HIDE:
            {
                _state = new HideState(gameObject.transform);
                break;
            }
            case States.WANDER:
            {
                _state = new WanderState(gameObject.transform);
                break;
            }
        }

        _justHidden = false;
        _wandering = false;

        _state.OnStateChangedEvent += HandleStateChangedEvent;
    }

    // what follows is mostly the same code as in the different bot behaviors inside Behaviors folder
    // things could have been better architectured so to don't have to dup this code, but it's just not the focus here

    public float Seek() // seeks the player and returns the current distance
    {
        _agent.SetDestination(Player.transform.position);

        return Vector3.Distance(Player.transform.position, transform.position);
    }

    public void Hide()
    {
        if (_agent.remainingDistance < 0.5f && CheckPlayerVisible())
        {
            var closestObstacle = FindClosestObstacleForward();
            var playerObstacleDir = closestObstacle.transform.position - Player.transform.position;
            playerObstacleDir.Normalize();

            RaycastHit[] hitInfos;
            hitInfos = Physics.RaycastAll(Player.transform.position, playerObstacleDir, 10.0f);
            foreach (var h in hitInfos)
            {
                if (GameObject.Equals(h.transform.gameObject, closestObstacle))
                {
                    var localPoint = h.transform.InverseTransformPoint(h.point);
                    var playerObstacleDirLocal = h.transform.InverseTransformDirection(playerObstacleDir).normalized;
                    
                    var oppositeLocalPoint = -localPoint;
                    var traverseVector = oppositeLocalPoint - localPoint;

                    var angle = Vector3.Angle(playerObstacleDirLocal, traverseVector.normalized) * Mathf.Deg2Rad;
                    var distanceFromHit = traverseVector.magnitude / Mathf.Cos(angle);

                    var localBackHitPoint = localPoint + playerObstacleDirLocal * distanceFromHit;
                    var backHitPoint = h.transform.TransformPoint(localBackHitPoint);

                    _agent.SetDestination(backHitPoint);

                    break;
                }
            }
        }
        else
        {
            if (!CheckPlayerVisible() && !_justHidden) // have just hidden from player, raise the "Ok, hidden" event
            {
                _justHidden = true;
                OnHiddenFromPlayer.Invoke();
            }
        }
    }

    private bool CheckPlayerVisible()
    {
        Vector3 rayDir = Player.transform.position - transform.position;
        RaycastHit hitInfo;

        if (rayDir.magnitude < 1.0f)
            return true;
        
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

    public void KeepHiddenFor(float secs)
    {
        Invoke("RaiseHiddenCompletedEvent", secs);
    }

    void RaiseHiddenCompletedEvent()
    {
        OnKeepHiddenCompleted.Invoke();
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 point)
    {
        var randCircle = UnityEngine.Random.insideUnitCircle * range;
        var rand = new Vector3(randCircle.x, 0.0f, randCircle.y);
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(rand, out hit, 1.0f, NavMesh.GetAreaFromName("Wakable"))) // check if the position belongs to the NavMesh
        {
            point = hit.position;
            return true;
        }

        point = Vector3.zero;
        return false;
    }

    private void Wander()
    {
        Vector3 randomPoint;
        if (!_agent.hasPath || _agent.remainingDistance < 1f)
        {
            if (RandomPoint(transform.position, WanderRange, out randomPoint))
            {
                _agent.SetDestination(randomPoint);
            }
        }
    }

    public void WanderFor(float secs)
    {
        if (!_wandering)
        {
            _wandering = true;
            Invoke("RaiseWanderCompletedEvent", secs);
        }
        else
        {
            Wander();
        }
    }

    void RaiseWanderCompletedEvent()
    {
        OnWanderCompleted.Invoke();
    }
}
