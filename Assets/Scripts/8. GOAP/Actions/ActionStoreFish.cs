using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionStoreFish : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionStoreFish");
        }

        public override bool Run()
        {
            return StoreFish();
        }

        private bool StoreFish()
        {
            Debug.Log("Fish stored!");

            return true;
        }
    }
}
