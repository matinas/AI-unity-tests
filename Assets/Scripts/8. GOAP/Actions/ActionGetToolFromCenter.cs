using System;
using System.Collections;
using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionGetToolFromCenter : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionGetToolFromCenter");

            if (Preconditions == null) // if there are no preconditions from the inspector, fill them manually
            {
                AddPrecondition(WorldStateAttribute.ToolAvailableInCenter, (Func<bool>) CheckToolAvailable);
                AddPrecondition(WorldStateAttribute.HasTool, false);
            }
            
            if (Effects == null) // if there are no effects from the inspector, fill them manually
            {
                AddEffect(WorldStateAttribute.HasTool, true);  
            }

            // WE CAN USE THIS TO CHECK WHETHER A PRECONDITION IS PROCEDURAL OR NOT...
            // if (value.GetType().IsPrimitive) // primitive type, commonly a bool for now
            // if (value.GetType() == typeof(Action)) // Action type, a boolean function
            // {

            // }
        }

        public override bool Run()
        {
            if (base.Run())
            {
                StorageManager.Instance.GetTool(1);
                return true;
            }

            return false;
        }

        private bool CheckToolAvailable()
        {
            return StorageManager.Instance.ToolAmount > 0;
        }
    }
}
