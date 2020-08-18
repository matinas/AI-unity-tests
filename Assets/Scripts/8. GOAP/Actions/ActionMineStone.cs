using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionMineStone : GOAPAction
    {
        public override void SetFixedPreconditions()
        {
            Debug.Log("Init ActionMineStone preconditions");

            Preconditions.AddState(WorldStateAttribute.ToolEquipped, true);
        }

        public override void SetFixedEffects()
        {
            Debug.Log("Init ActionMineStone effects");

            Effects.AddState(WorldStateAttribute.StoneCollected, true);
        }
    }
}
