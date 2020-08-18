using System.Collections;
using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionEquipTool : GOAPAction
    {
        public override void SetFixedPreconditions()
        {
            Debug.Log("Init ActionEquipTool preconditions");

            Preconditions.AddState(WorldStateAttribute.HasTool, true);
        }

        public override void SetFixedEffects()
        {
            Debug.Log("Init ActionEquipTool effects");

            Effects.AddState(WorldStateAttribute.ToolEquipped, true);  
        }
    }
}
