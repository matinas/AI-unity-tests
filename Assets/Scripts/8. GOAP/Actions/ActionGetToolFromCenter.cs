using System;
using System.Collections;
using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionGetToolFromCenter : GOAPAction
    {
        public override void SetFixedPreconditions()
        {
            Debug.Log("Init ActionGetToolFromCenter preconditions");

            Preconditions.AddState(WorldStateAttribute.HasTool, false);
        }

        public override void SetProceduralPreconditions()
        {
            Preconditions.AddState(WorldStateAttribute.ToolAvailableInCenter, (Func<bool>) CheckToolAvailable); // we always need to manually add procedural precondition

            // WE CAN USE THIS TO CHECK WHETHER A PRECONDITION IS PROCEDURAL OR NOT...
            // if (value.GetType().IsPrimitive) // primitive type, commonly a bool for now
            // if (value.GetType() == typeof(Action)) // Action type, a boolean function
            // {

            // }
        }

        public override void SetFixedEffects()
        {
            Debug.Log("Init ActionGetToolFromCenter effects");

            Effects.AddState(WorldStateAttribute.HasTool, true);
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
