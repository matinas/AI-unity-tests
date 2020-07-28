using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionWaitForMaterials : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionWaitForMaterials");
        }

        public override bool Run()
        {
            return WaitForMaterials();
        }

        private bool WaitForMaterials()
        {
            Debug.Log("Waited for material!");

            return true;
        }
    }
}
