using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionWaitForMaterials : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionWaitForMaterials");

            AddPrecondition(WorldStateAttribute.MaterialsAvailableForTool, false);
            AddPrecondition(WorldStateAttribute.HasTool, false);
            AddEffect(WorldStateAttribute.MaterialsAvailableForTool, true);
        }

        public override bool Run()
        {
            Debug.Log("Waited for material!");

            return true;
        }
    }
}
