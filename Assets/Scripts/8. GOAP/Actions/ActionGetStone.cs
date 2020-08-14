using System;
using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionGetStone : GOAPAction
    {
        public event Action OnStoneCollected;

        public override void Init()
        {
            Debug.Log("Init ActionGetStone");

            if (Preconditions == null) // if there are no preconditions from the inspector, fill them manually
            {
                AddPrecondition(WorldStateAttribute.StoneCollected, true);
            }
            
            if (Effects == null) // if there are no effects from the inspector, fill them manually
            {
                AddEffect(WorldStateAttribute.StoneStored, true);   
            }
        }

        public override bool Run()
        {
            if (base.Run())
            {
                StorageManager.Instance.RegisterStone(10);
                return true;
            }

            return false;
        }
    }
}
