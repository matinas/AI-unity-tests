using UnityEngine;
using AITests.GOAP;

public class WorldManager : MonoBehaviour
{
    private static WorldManager _instance;

    public static WorldManager Instance
    {
        get
        {
            if (_instance == null) Debug.Log("Please make sure that the WorldManager is added to the scene");

            return _instance;
        }
    }

    public WorldState GlobalState { get; private set; }

    void Awake()
    {
        _instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GlobalState = new WorldState();
        GlobalState.AddState(WorldStateAttribute.MaterialsAvailableForTool, false);
        GlobalState.AddState(WorldStateAttribute.ToolAvailableInCenter, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
