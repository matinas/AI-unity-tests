using UnityEngine;
using UnityEngine.AI;

public class EvadeBot : MonoBehaviour
{
    private NavMeshAgent _agent;

    [SerializeField]
    private Transform Player;

    [SerializeField]
    [Range(0.0f,2.0f)]
    [Tooltip("Amount of look ahead of the bot. The higher this attribute the higher the bot's confidence on that the player will follow the current direction (so it will try to anticipate or look ahead more)")]
    private float LookAhead;

    [SerializeField]
    private float MaxSeekAngle;

    [SerializeField]
    private float MaxAllowedDistance;

    private Vector3 _playerPrevPosition;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerPrevPosition = Player.transform.position;
    }

    private void Evade()
    {
        var playerDisplacement = Player.transform.position - _playerPrevPosition; // current displacement from previous frame to this frame
        var playerVelocity = playerDisplacement / Time.deltaTime;                 // current velocity vector
        _playerPrevPosition = Player.transform.position;                          // cache the last position, so we can calculate the displacement and thus velocity the next frame

        Vector3 moveDir;
        var angle = Vector3.Angle(playerVelocity, transform.forward);
        float distance = Vector3.Distance(Player.transform.position, transform.position);

        if (angle >= 90 || angle <= MaxSeekAngle) // player is moving in the opposite direction of the bot or the angle is just too small to look ahead, so we switch to a simple Seek behavior
        {
            moveDir = Player.transform.position - transform.position;
        }
        else // player is moving in the same direction of the bot and angle is big enough, so we look ahead...
        {
            var predictedPos = Player.transform.position + playerVelocity * (distance * LookAhead / _agent.speed); // predicted position is based on the player's velocity
                                                                                                                   // and the bot's LookAhead attribute, speed and distance
            distance = Vector3.Distance(predictedPos, transform.position);
            moveDir = predictedPos - transform.position;
        }

        if (distance < MaxAllowedDistance)
        {
            _agent.SetDestination(transform.position - moveDir); // move the bot in the direction of the predicted position
        }
    }

    // Update is called once per frame
    void Update()
    {
        Evade();
    }
}
