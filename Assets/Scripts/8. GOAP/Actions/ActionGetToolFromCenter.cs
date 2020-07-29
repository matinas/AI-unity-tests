using System;
using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionGetToolFromCenter : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionGetToolFromCenter");

            AddPrecondition(WorldStateAttribute.ToolAvailableInCenter, CheckToolAvailable());
            AddPrecondition(WorldStateAttribute.HasTool, false);
            AddEffect(WorldStateAttribute.HasTool, true);

            // WE CAN USE THIS TO CHECK WHETHER A PRECONDITION IS PROCEDURAL OR NOT...
            // if (value.GetType().IsPrimitive) // primitive type, commonly a bool for now
            // if (value.GetType() == typeof(Action)) // Action type, a boolean function
            // {

            // }
        }

        public override bool Run()
        {
            Debug.Log("Got tool!");

            return true;
        }

        private bool CheckToolAvailable()
        {
            // TODO: query the WorldManager to check if there's a tool available

            throw new NotImplementedException();
        }
    }
}
