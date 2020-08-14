using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCatchFish : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionCatchFish");

            if (Preconditions == null) // if there are no preconditions from the inspector, fill them manually
            {
                AddPrecondition(WorldStateAttribute.ToolEquipped, true);
            }
            
            if (Effects == null) // if there are no effects from the inspector, fill them manually
            {
                AddEffect(WorldStateAttribute.FishCollected, true);
            }
        }

        public override bool Run()
        {
            return base.Run();
        }
    }
}
