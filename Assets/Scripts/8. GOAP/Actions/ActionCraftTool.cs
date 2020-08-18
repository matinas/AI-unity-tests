using System;
using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCraftTool : GOAPAction
    {
        public override void SetFixedPreconditions()
        {
            Debug.Log("Init ActionCraftTool preconditions");

            Preconditions.AddState(WorldStateAttribute.HasTool, false);
        }

        public override void SetProceduralPreconditions()
        {
            Preconditions.AddState(WorldStateAttribute.MaterialsAvailableForTool, (Func<bool>) CheckMaterialsAvailable);
        }

        public override void SetFixedEffects()
        {
            Debug.Log("Init ActionCraftTool effects");

            Effects.AddState(WorldStateAttribute.HasTool, true);
            Effects.AddState(WorldStateAttribute.ToolCrafted, true);
        }

        public override bool PreRun()
        {
            return StorageManager.Instance.GetWood(2) &&
                   StorageManager.Instance.GetStone(2);
        }

        private bool CheckMaterialsAvailable()
        {
            return StorageManager.Instance.WoodAmount > 2 &&
                   StorageManager.Instance.StoneAmount > 2;
        }
    }
}
