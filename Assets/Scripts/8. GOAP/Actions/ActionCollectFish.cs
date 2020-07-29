using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCollectFish : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionCollectFish");

            AddPrecondition(WorldStateAttribute.ToolEquipped, true);
            AddEffect(WorldStateAttribute.ResourceCollected, true);
        }

        public override bool Run()
        {
            Debug.Log("Fish collected!");

            return true;
        }
    }
}
