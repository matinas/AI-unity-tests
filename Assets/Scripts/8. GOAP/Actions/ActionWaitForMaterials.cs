using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionWaitForMaterials : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionWaitForMaterials");

            AddPrecondition(WorldStateAttribute.MaterialsAvailable, false);
            AddEffect(WorldStateAttribute.MaterialsAvailable, true);
        }

        public override bool Run()
        {
            Debug.Log("Waited for material!");

            return true;
        }
    }
}
