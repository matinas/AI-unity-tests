using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionStoreToolInCenter : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionStoreToolInCenter");

            if (Preconditions == null) // if there are no preconditions from the inspector, fill them manually
            {
                AddPrecondition(WorldStateAttribute.ToolCrafted, true);
            }
            
            if (Effects == null) // if there are no effects from the inspector, fill them manually
            {
                AddEffect(WorldStateAttribute.ToolAvailableInCenter, true);
            }
        }

        public override bool Run()
        {
            return base.Run();
        }
    }
}
