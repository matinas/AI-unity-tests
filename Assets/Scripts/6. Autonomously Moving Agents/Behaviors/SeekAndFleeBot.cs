using UnityEngine;
using UnityEngine.AI;

public class SeekAndFleeBot : MonoBehaviour
{
    private NavMeshAgent _agent;

    [SerializeField]
    private Transform Player;

    private bool _isBeingChased = false;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Seek()
    {
        _agent.SetDestination(Player.transform.position);
    }

    private void Flee()
    {
        var playerDir = Player.transform.position - transform.position;
        var dest = -playerDir; // calculate the flee destination in the opposite way as the player/chaser

        _agent.SetDestination(dest);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isBeingChased = !_isBeingChased;
        }

        if (_isBeingChased)
        {
            Flee();
        }
        else
        {
            Seek();
        }
    }
}
