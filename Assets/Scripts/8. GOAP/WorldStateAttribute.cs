using System;

namespace AITests.GOAP
{
    [Serializable]
    public enum WorldStateAttribute
    {
        ToolEquipped,
        MaterialsAvailableForTool,
        ToolAvailable,
        ResourceCollected,
        InCollectFishRange,
        InCollectWoodRange,
        InCollectStoneRange,
        InToolCraftingRange,
        InWaitingMaterialsRange
    }
}

