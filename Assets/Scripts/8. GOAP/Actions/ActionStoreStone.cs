using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionStoreStone : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionStoreStone");

            AddPrecondition(WorldStateAttribute.ResourceCollected, true);
            AddEffect(WorldStateAttribute.ResourceStored, true);
        }

        public override bool Run()
        {
            Debug.Log("Stone stored!");

            return true;
        }
    }
}
