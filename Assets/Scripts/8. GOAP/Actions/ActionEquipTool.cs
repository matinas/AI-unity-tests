using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionEquipTool : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionEquipTool");
        }

        public override bool Run()
        {
            return EquipTool();
        }

        private bool EquipTool()
        {
            Debug.Log("Tool equipped!");

            return true;
        }
    }
}
