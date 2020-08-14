using UnityEngine;

namespace AITests.GOAP.Actions
{
    public class ActionGetWood : GOAPAction
    {
        public override void Init()
        {
            Debug.Log("Init ActionGetWood");

            if (Preconditions == null) // if there are no preconditions from the inspector, fill them manually
            {
                AddPrecondition(WorldStateAttribute.WoodCollected, true);
            }
            
            if (Effects == null) // if there are no effects from the inspector, fill them manually
            {
                AddEffect(WorldStateAttribute.WoodStored, true);   
            }
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
