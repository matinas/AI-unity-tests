using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCutWood : GOAPAction
    {
        public override void SetFixedPreconditions()
        {
            Debug.Log("Init ActionCutWood preconditions");

            Preconditions.AddState(WorldStateAttribute.ToolEquipped, true);
        }

        public override void SetFixedEffects()
        {
            Debug.Log("Init ActionCutWood effects");

            Effects.AddState(WorldStateAttribute.WoodCollected, true);
        }

        public override bool Run()
        {
            return base.Run();
        }
    }
}
