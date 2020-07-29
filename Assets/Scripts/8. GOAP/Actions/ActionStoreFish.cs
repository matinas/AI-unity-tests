using System;
using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionStoreFish : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionStoreFish");

            AddPrecondition(WorldStateAttribute.ResourceCollected, true);
            AddEffect(WorldStateAttribute.ResourceStored, true);
        }

        public override bool Run()
        {
            Debug.Log("Fish stored!");

            return true;
        }
    }
}
