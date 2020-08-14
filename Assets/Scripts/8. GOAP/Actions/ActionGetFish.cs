using System;
using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionGetFish : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionGetFish");

            if (Preconditions == null) // if there are no preconditions from the inspector, fill them manually
            {
                AddPrecondition(WorldStateAttribute.FishCollected, true);
            }
            
            if (Effects == null) // if there are no effects from the inspector, fill them manually
            {
                AddEffect(WorldStateAttribute.FishStored, true);    
            }
        }

        public override bool Run()
        {
            if (base.Run())
            {
                StorageManager.Instance.RegisterFish(10);
                return true;
            }

            return false;
        }
    }
}
