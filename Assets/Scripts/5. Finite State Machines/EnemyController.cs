using UnityEngine;
using AITests.FSM.States;
using UnityEngine.AI;
using System.Collections.Generic;
using AITests.Pathfinding;
using System;
using System.Linq;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public event Action<int> OnWaypointReached;
    public event Action OnSpinAroundCompleted;
    public event Action OnChasePlayerCompleted;
    public event Action OnNapCompleted;

    [SerializeField]
    private Transform Player;

    [SerializeField]
    private float SpinSpeed;

    private IState _state;

    private NavMeshAgent _navmeshAgent;

    private bool _wpReached = false;
    private int _currentWp; 

    private IEnumerable<WaypointNode> _waypoints; // these waypoints are set in the scene based on the "Scene graph.png" graph inside "3. Pathfinding" folder

    private Coroutine _spinCrt, _chaseCrt, _napCrt;

    // Start is called before the first frame update
    void Start()
    {
        _waypoints = GameObject.FindObjectsOfType<WaypointNode>().OrderBy(w => w.ID);
        _navmeshAgent = GetComponent<NavMeshAgent>();

        _state = new IdleState(gameObject.transform);
        _state.OnStateChangedEvent += HandleStateChangedEvent;
    }

    // Update is called once per frame
    void Update()
    {
        _state.Update();

        if (Vector3.Distance(transform.position, _waypoints.ElementAt(_currentWp).transform.position) < 1.0f && !_wpReached)
        {
            OnWaypointReached?.Invoke(_currentWp);
            _wpReached = true;
        }
    }

    private void HandleStateChangedEvent(StateChangedEvt e)
    {
        _state.OnStateChangedEvent -= HandleStateChangedEvent; // remove old state handler

        switch (e.newState)
        {
            case States.IDLE:
            {
                _state = new IdleState(gameObject.transform); // we could do something to avoid instancing this every time (Factory?) but
                break;                                        // we're doing it this way for the sake of simplicity, so to focus on the State Pattern
            }
            case States.PATROL:
            {
                _state = new PatrolState(gameObject.transform);
                break;
            }
            case States.CHASE:
            {
                _state = new ChaseState(gameObject.transform);
                break;
            }
            case States.SLEEP:
            {
                _state = new SleepState(gameObject.transform);
                break;
            }
        }

        _state.OnStateChangedEvent += HandleStateChangedEvent; // add new state handler
    }

    public float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, Player.transform.position);
    }

    public void SetDestination(Vector3 destination)
    {
        _navmeshAgent.SetDestination(destination);
    }

    public void GoToWaypoint(int wp)
    {
        _wpReached = false;
        _currentWp = wp;
        _navmeshAgent.SetDestination(_waypoints.ElementAt(wp).transform.position);
    }

    public int WaypointCout()
    {
        return _waypoints.Count();
    }

    public void SpinAround(float secs)
    {
        if (_spinCrt == null)
        {
            _spinCrt = StartCoroutine(SpinAroundCrt(secs));
        }
    }

    IEnumerator SpinAroundCrt(float secs)
    {
        float time = secs;

        while (time > 0)
        {
            transform.Rotate(0.0f, Time.deltaTime * SpinSpeed, 0.0f);
            time -= Time.deltaTime;

            yield return null;
        }

        OnSpinAroundCompleted.Invoke(); // FIXME: this call works, but it's not correct, as it will be executed multiple times (once for each iteration)
        _spinCrt = null;
    }

    public void ChasePlayer(float secs)
    {
        if (_chaseCrt == null)
        {
            _chaseCrt = StartCoroutine(ChasePlayerCrt(secs));
        }
    }

    IEnumerator ChasePlayerCrt(float secs)
    {
        float time = secs;

        while (time > 0)
        {
            _navmeshAgent.SetDestination(Player.transform.position);
            time -= Time.deltaTime;

            yield return null;
        }

        OnChasePlayerCompleted.Invoke(); // FIXME: this call works, but it's not correct, as it will be executed multiple times (once for each iteration)
        _chaseCrt = null;
    }

    public void TakeANap(float secs)
    {
        if (_napCrt == null)
        {
            _napCrt = StartCoroutine(TakeANapCrt(secs));
        }
    }

    IEnumerator TakeANapCrt(float secs)
    {
        float time = secs;
        while (time > 0)
        {
            time -= Time.deltaTime;

            yield return null;
        }

        OnNapCompleted.Invoke();
        _napCrt = null;
    }
}
