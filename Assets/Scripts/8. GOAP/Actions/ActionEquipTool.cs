using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionEquipTool : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionEquipTool");

            AddPrecondition(WorldStateAttribute.HasTool, true);
            AddEffect(WorldStateAttribute.ToolEquipped, true);
        }

        public override bool Run()
        {
            Debug.Log("Tool equipped!");

            return true;
        }
    }
}
