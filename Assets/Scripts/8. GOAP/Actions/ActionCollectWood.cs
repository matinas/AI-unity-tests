using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCollectWood : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionCollectWood");
        }

        public override bool Run()
        {
            return CollectWood();
        }

        private bool CollectWood()
        {
            Debug.Log("Wood collected!");

            return true;
        }
    }
}
