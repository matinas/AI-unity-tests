using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionStoreWood : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionStoreWood");

            AddPrecondition(WorldStateAttribute.ResourceCollected, true);
            AddEffect(WorldStateAttribute.ResourceStored, true);
        }

        public override bool Run()
        {
            Debug.Log("Wood stored!");

            return true;
        }
    }
}
