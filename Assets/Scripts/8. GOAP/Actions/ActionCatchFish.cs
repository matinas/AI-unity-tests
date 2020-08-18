using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCatchFish : GOAPAction
    {
        public override void SetFixedPreconditions()
        {
            Debug.Log("Init ActionCatchFish preconditions");

            Preconditions.AddState(WorldStateAttribute.ToolEquipped, true);
        }

        public override void SetFixedEffects()
        {
            Debug.Log("Init ActionCatchFish effects");

            Effects.AddState(WorldStateAttribute.FishCollected, true);
        }
    }
}
