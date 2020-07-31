using System;
using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCraftTool : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionCraftTool");

            AddPrecondition(WorldStateAttribute.MaterialsAvailableForTool, CheckMaterialsAvailable());
            AddPrecondition(WorldStateAttribute.HasTool, false);
            AddEffect(WorldStateAttribute.HasTool, true);
        }

        public override bool Run()
        {
            Debug.Log("Tool crafted!");

            return true;
        }

        private bool CheckMaterialsAvailable()
        {
            throw new NotImplementedException();
        }
    }
}
