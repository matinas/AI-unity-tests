using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionStoreWood : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionStoreWood");
        }

        public override bool Run()
        {
            return StoreWood();
        }

        private bool StoreWood()
        {
            Debug.Log("Wood stored!");

            return true;
        }
    }
}
