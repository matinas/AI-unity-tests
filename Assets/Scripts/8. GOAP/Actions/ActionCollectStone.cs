using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCollectStone : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionCollectStone");

            AddPrecondition(WorldStateAttribute.ToolEquipped, true);
            AddEffect(WorldStateAttribute.ResourceCollected, true);
        }

        public override bool Run()
        {
            Debug.Log("Stone collected!");

            return true;
        }
    }
}
