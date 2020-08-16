using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionGetFish : GOAPAction
    {
        public override void SetFixedPreconditions()
        {
            Debug.Log("Init ActionGetFish preconditions");

            Preconditions.AddState(WorldStateAttribute.FishCollected, true);
        }

        public override void SetFixedEffects()
        {
            Debug.Log("Init ActionGetFish effects");

            Effects.AddState(WorldStateAttribute.FishStored, true); 
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
