using System;
using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionGetStone : GOAPAction
    {
        public override void SetFixedPreconditions()
        {
            Debug.Log("Init ActionGetStone preconditions");

            Preconditions.AddState(WorldStateAttribute.StoneCollected, true);
        }

        public override void SetFixedEffects()
        {
            Debug.Log("Init ActionGetStone effects");

            Effects.AddState(WorldStateAttribute.StoneStored, true);   
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
