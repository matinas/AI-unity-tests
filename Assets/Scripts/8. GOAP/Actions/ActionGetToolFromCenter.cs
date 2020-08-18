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
            Preconditions.AddState(WorldStateAttribute.ToolAvailableInCenter, (Func<bool>) CheckToolAvailable);
        }

        public override void SetFixedEffects()
        {
            Debug.Log("Init ActionGetToolFromCenter effects");

            Effects.AddState(WorldStateAttribute.HasTool, true);
        }

        public override bool PostRun()
        {
            StorageManager.Instance.GetTool(1);

            return true;
        }

        private bool CheckToolAvailable()
        {
            return StorageManager.Instance.ToolAmount > 0;
        }
    }
}
