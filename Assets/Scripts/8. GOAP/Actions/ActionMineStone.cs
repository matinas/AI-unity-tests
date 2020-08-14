using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionMineStone : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionMineStone");

            if (Preconditions == null) // if there are no preconditions from the inspector, fill them manually
            {
                AddPrecondition(WorldStateAttribute.ToolEquipped, true);
            }
            
            if (Effects == null) // if there are no effects from the inspector, fill them manually
            {
                AddEffect(WorldStateAttribute.StoneCollected, true);
            }
        }

        public override bool Run()
        {
            return base.Run();
        }
    }
}
