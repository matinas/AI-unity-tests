using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCutWood : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionCutWood");

            AddPrecondition(WorldStateAttribute.ToolEquipped, true);
            AddEffect(WorldStateAttribute.WoodCollected, true);
        }

        public override bool Run()
        {
            Debug.Log("Wood collected!");

            return true;
        }
    }
}
