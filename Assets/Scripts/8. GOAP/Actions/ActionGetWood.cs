using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionGetWood : GOAPAction
    {
        public override void SetFixedPreconditions()
        {
            Debug.Log("Init ActionGetWood preconditions");

            Preconditions.AddState(WorldStateAttribute.WoodCollected, true);
        }

        public override void SetFixedEffects()
        {
            Debug.Log("Init ActionGetWood effects");

            Effects.AddState(WorldStateAttribute.WoodStored, true);  
        }

        public override bool Run()
        {
            if (base.Run())
            {
                StorageManager.Instance.RegisterWood(10);
                return true;
            }

            return false;
        }
    }
}
