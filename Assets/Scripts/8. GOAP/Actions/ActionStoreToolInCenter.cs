using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionStoreToolInCenter : GOAPAction
    {
        public override void SetFixedPreconditions()
        {
            Debug.Log("Init ActionStoreToolInCenter preconditions");

            Preconditions.AddState(WorldStateAttribute.ToolCrafted, true);
        }

        public override void SetFixedEffects()
        {
            Debug.Log("Init ActionStoreToolInCenter effects");

            Effects.AddState(WorldStateAttribute.ToolAvailableInCenter, true);
        }

        public override bool PostRun()
        {
            StorageManager.Instance.RegisterTool(1);

            return true;
        }

        public override bool Run()
        {
            return base.Run();
        }
    }
}
