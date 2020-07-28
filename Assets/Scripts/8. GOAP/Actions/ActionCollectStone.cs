using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionCollectStone : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionCollectStone");
        }

        public override bool Run()
        {
            return CollectStone();
        }

        private bool CollectStone()
        {
            Debug.Log("Stone collected!");

            return true;
        }
    }
}
