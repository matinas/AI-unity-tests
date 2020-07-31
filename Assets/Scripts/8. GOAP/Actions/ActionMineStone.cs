using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionMineStone : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionMineStone");

            AddPrecondition(WorldStateAttribute.ToolEquipped, true);
            AddEffect(WorldStateAttribute.StoneCollected, true);
        }

        public override bool Run()
        {
            Debug.Log("Stone collected!");

            return true;
        }
    }
}
