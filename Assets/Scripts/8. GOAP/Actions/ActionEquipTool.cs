﻿using System.Collections;
using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionEquipTool : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionEquipTool");

            if (Preconditions == null) // if there are no preconditions from the inspector, fill them manually
            {
                AddPrecondition(WorldStateAttribute.HasTool, true);
            }
            
            if (Effects == null) // if there are no effects from the inspector, fill them manually
            {
                AddEffect(WorldStateAttribute.ToolEquipped, true);  
            }
        }

        public override bool Run()
        {
            return base.Run();
        }
    }
}
