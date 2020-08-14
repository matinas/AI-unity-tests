using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionWaitForMaterials : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionWaitForMaterials");

            if (Preconditions == null) // if there are no preconditions from the inspector, fill them manually
            {
                AddPrecondition(WorldStateAttribute.MaterialsAvailableForTool, false);
                AddPrecondition(WorldStateAttribute.HasTool, false);
            }
            
            if (Effects == null) // if there are no effects from the inspector, fill them manually
            {
                AddEffect(WorldStateAttribute.MaterialsAvailableForTool, true); 
            }
        }

        public override bool Run()
        {
            return base.Run();
        }
    }
}
