using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionWaitForMaterials : GOAPAction
    {
        public override void SetFixedPreconditions()
        {
            Debug.Log("Init ActionWaitForMaterials preconditions");

            Preconditions.AddState(WorldStateAttribute.MaterialsAvailableForTool, false);
            Preconditions.AddState(WorldStateAttribute.HasTool, false);
        }

        public override void SetFixedEffects()
        {
            Debug.Log("Init ActionWaitForMaterials effects");

            Effects.AddState(WorldStateAttribute.MaterialsAvailableForTool, true); 
        }

        public override bool Run()
        {
            return base.Run();
        }
    }
}
