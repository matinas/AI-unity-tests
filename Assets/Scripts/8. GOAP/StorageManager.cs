using UnityEngine;

public class StorageManager : MonoBehaviour
{
    private static StorageManager _instance;

    public static StorageManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("Please make sure that the StorageManager is added to the scene");

            return _instance;
        }
    }

    [SerializeField]
    private int StoneAmount_Debug, FishAmount_Debug, WoodAmount_Debug, ToolAmount_Debug;

    public int StoneAmount { get; private set; }
    public int FishAmount { get; private set; }
    public int WoodAmount { get; private set; }
    public int ToolAmount { get; private set; }

    void Awake()
    {
        _instance = this;
    }
    
    void Start()
    {
        StoneAmount = StoneAmount_Debug; // = 0;
        FishAmount = FishAmount_Debug; // = 0;
        WoodAmount = WoodAmount_Debug; //= 0;
        ToolAmount = ToolAmount_Debug; // = 0;
    }

    public void RegisterStone(int amount)
    {
        StoneAmount += amount;
        StoneAmount_Debug = StoneAmount;
    }

    public void RegisterFish(int amount)
    {
        FishAmount += amount;
        FishAmount_Debug = FishAmount;
    }

    public void RegisterWood(int amount)
    {
        WoodAmount += amount;
        WoodAmount_Debug = WoodAmount;
    }

    public void RegisterTool(int amount)
    {
        ToolAmount += amount;
        ToolAmount_Debug = ToolAmount;
    }

    public bool GetStone(int amount)
    {
        if (amount <= StoneAmount)
        {
            StoneAmount -= amount;
            StoneAmount_Debug = StoneAmount;

            return true;
        }
        
        return false;
    }

    public bool GetFish(int amount)
    {
        if (amount <= FishAmount)
        {
            FishAmount -= amount;
            FishAmount_Debug = FishAmount;

            return true;
        }
        
        return false;
    }

    public bool GetWood(int amount)
    {
        if (amount <= WoodAmount)
        {
            WoodAmount -= amount;
            WoodAmount_Debug = WoodAmount;

            return true;
        }
        
        return false;
    }

    public bool GetTool(int amount)
    {
        if (amount <= ToolAmount)
        {
            ToolAmount -= amount;
            ToolAmount_Debug = ToolAmount;

            return true;
        }

        return false;
    }
}
