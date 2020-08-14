using System;
using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCraftTool : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionCraftTool");

            if (Preconditions == null) // if there are no preconditions from the inspector, fill them manually
            {
                AddPrecondition(WorldStateAttribute.MaterialsAvailableForTool, (Func<bool>) CheckMaterialsAvailable);
                AddPrecondition(WorldStateAttribute.HasTool, false);
            }
            
            if (Effects == null) // if there are no effects from the inspector, fill them manually
            {
                AddEffect(WorldStateAttribute.HasTool, true);
                AddEffect(WorldStateAttribute.ToolCrafted, true);
            }
        }

        public override bool PreRun()
        {
            return StorageManager.Instance.GetWood(2) &&
                   StorageManager.Instance.GetStone(2);
        }

        public override bool Run()
        {
            return base.Run();
        }

        public override bool PostRun()
        {
            StorageManager.Instance.RegisterTool(1);

            return true;
        }

        private bool CheckMaterialsAvailable()
        {
            return StorageManager.Instance.WoodAmount > 2 &&
                   StorageManager.Instance.StoneAmount > 2;
        }
    }
}
