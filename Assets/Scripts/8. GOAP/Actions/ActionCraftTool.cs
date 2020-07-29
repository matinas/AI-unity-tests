using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCraftTool : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionCraftTool");

            AddPrecondition(WorldStateAttribute.MaterialsAvailable, true);
            AddPrecondition(WorldStateAttribute.HasTool, false);
            AddEffect(WorldStateAttribute.HasTool, true);
        }

        public override bool Run()
        {
            Debug.Log("Tool crafted!");

            return true;
        }
    }
}
