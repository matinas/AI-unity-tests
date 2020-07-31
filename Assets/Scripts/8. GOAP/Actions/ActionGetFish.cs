using System;
using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionGetFish : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionGetFish");

            AddPrecondition(WorldStateAttribute.FishCollected, true);
            AddEffect(WorldStateAttribute.FishStored, true);
        }

        public override bool Run()
        {
            Debug.Log("Got fish!");

            return true;
        }
    }
}
