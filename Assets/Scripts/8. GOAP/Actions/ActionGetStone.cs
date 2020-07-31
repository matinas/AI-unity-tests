using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionGetStone : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionGetStone");

            AddPrecondition(WorldStateAttribute.StoneCollected, true);
            AddEffect(WorldStateAttribute.StoneStored, true);
        }

        public override bool Run()
        {
            Debug.Log("Got stone!");

            return true;
        }
    }
}
