using System;
using UnityEngine;

namespace AITests.GOAP
{
    [Serializable]
    public enum WorldStateAttribute
    {
        ToolEquipped,
        MaterialsAvailable,
        ToolAvailableInCenter,
        HasTool,
        ResourceCollected,
        ResourceStored,
    }
}

