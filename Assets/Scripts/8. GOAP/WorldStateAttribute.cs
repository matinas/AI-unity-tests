using System;
using UnityEngine;

namespace AITests.GOAP
{
    [Serializable]
    public enum WorldStateAttribute
    {
        ToolEquipped,
        MaterialsAvailableForTool,
        ToolAvailableInCenter,
        HasTool,
        StoneCollected,
        FishCollected,
        WoodCollected,
        StoneStored,
        FishStored,
        WoodStored,
    }
}

