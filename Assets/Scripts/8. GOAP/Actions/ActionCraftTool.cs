using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCraftTool : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionCraftTool");
        }

        public override bool Run()
        {
            return CraftTool();
        }

        private bool CraftTool()
        {
            Debug.Log("Tool crafted!");

            return true;
        }
    }
}
