using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCollectWood : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionCollectWood");

            AddPrecondition(WorldStateAttribute.ToolEquipped, true);
            AddEffect(WorldStateAttribute.ResourceCollected, true);
        }

        public override bool Run()
        {
            Debug.Log("Wood collected!");

            return true;
        }
    }
}
