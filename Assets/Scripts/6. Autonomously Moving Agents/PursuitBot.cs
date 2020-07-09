using UnityEngine;
using UnityEngine.AI;

public class PursuitBot : MonoBehaviour
{
    private NavMeshAgent _agent;

    [SerializeField]
    private Transform Player;

    [SerializeField]
    [Range(0.0f,2.0f)]
    [Tooltip("Amount of look ahead of the bot. The higher this attribute the higher the bot's confidence on that the player will follow the current direction (so it will try to anticipate or look ahead more)")]
    private float LookAhead;

    private Vector3 _playerPrevPosition;

    // Debug.Log($"Player displacement: ({playerDisplacement.x}, {playerDisplacement.y}, {playerDisplacement.z})");
    // Debug.Log($"Player velocity: ({playerVelocity.x}, {playerVelocity.y}, {playerVelocity.z})");
    // Debug.DrawRay(Player.transform.position, predictedPos, Color.green, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerPrevPosition = Player.transform.position;
    }

    private void Pursuit()
    {
        var playerDisplacement = Player.transform.position - _playerPrevPosition; // current displacement from previous frame to this frame
        var playerVelocity = playerDisplacement / Time.deltaTime; // current velocity vector
        _playerPrevPosition = Player.transform.position;
        
        var predictedPos = Player.transform.position + playerVelocity * LookAhead; // predicted position based on the player's velocity and the bot LookAhead attribute
        var moveDir = predictedPos - transform.position;

        _agent.SetDestination(transform.position + moveDir); // move the bot in the direction of the predicted position
    }

    // Update is called once per frame
    void Update()
    {
        Pursuit();
    }
}
