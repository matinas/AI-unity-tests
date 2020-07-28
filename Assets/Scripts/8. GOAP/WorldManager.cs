using System.Collections.Generic;
using UnityEngine;
using AITests.GOAP;

public class WorldManager : MonoBehaviour
{
    private static WorldManager _instance;

    public static WorldManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("Please make sure that the WorldManager is added to the scene");

            return _instance;
        }
    }

    public Dictionary<WorldStateAttribute, object> WorldState { get; private set; }

    void Awake()
    {
        _instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillWorldState(ref Dictionary<WorldStateAttribute, object> worldState)
    {
        // TODO: lopp through the global current world state and fill the gaps in the worldState passed as parameter
    }
}
