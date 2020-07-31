using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionGetWood : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionGetWood");

            AddPrecondition(WorldStateAttribute.WoodCollected, true);
            AddEffect(WorldStateAttribute.WoodStored, true);
        }

        public override bool Run()
        {
            Debug.Log("Got wood!");

            return true;
        }
    }
}
