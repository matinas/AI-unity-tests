using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCollectFish : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionCollectFish");
        }

        public override bool Run()
        {
            return CollectFish();
        }

        private bool CollectFish()
        {
            Debug.Log("Fish collected!");

            return true;
        }
    }
}
