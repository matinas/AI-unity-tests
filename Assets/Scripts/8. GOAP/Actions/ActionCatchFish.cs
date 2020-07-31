using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCatchFish : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionCatchFish");

            AddPrecondition(WorldStateAttribute.ToolEquipped, true);
            AddEffect(WorldStateAttribute.FishCollected, true);
        }

        public override bool Run()
        {
            Debug.Log("Fish collected!");

            return true;
        }
    }
}
