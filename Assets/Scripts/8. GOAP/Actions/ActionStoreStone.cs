using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionStoreStone : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionStoreStone");
        }

        public override bool Run()
        {
            return StoreStone();
        }

        private bool StoreStone()
        {
            Debug.Log("Stone stored!");

            return true;
        }
    }
}
