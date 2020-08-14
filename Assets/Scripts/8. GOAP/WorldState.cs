using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AITests.GOAP
{
    [System.Serializable]
    public enum WorldStateAttribute
    {
        ToolEquipped,
        MaterialsAvailableForTool,
        ToolAvailableInCenter,
        HasTool,
        StoneCollected,
        FishCollected,
        WoodCollected,
        StoneStored,
        FishStored,
        WoodStored,
        ToolCrafted
    }

    [System.Serializable]
    public class WorldStatePair
    {
        public WorldStateAttribute Attr;
        public int Value;

        public WorldStatePair(WorldStateAttribute attr, int value)
        {
            Attr = attr;
            Value = value;
        }
    }

    [System.Serializable]
    public class WorldState
    {
        public Dictionary<WorldStateAttribute, object> States { get; private set; }

        public WorldState()
        {
            States = new Dictionary<WorldStateAttribute, object>();

            // AddState(WorldStateAttribute.ToolEquipped, false);
            // AddState(WorldStateAttribute.MaterialsAvailableForTool, false);
            // AddState(WorldStateAttribute.ToolAvailableInCenter, false);
            // AddState(WorldStateAttribute.HasTool, false);
            // AddState(WorldStateAttribute.StoneCollected, false);
            // AddState(WorldStateAttribute.FishCollected, false);
            // AddState(WorldStateAttribute.WoodCollected, false);
            // AddState(WorldStateAttribute.StoneStored, false);
            // AddState(WorldStateAttribute.FishStored, false);
            // AddState(WorldStateAttribute.WoodStored, false);
            // AddState(WorldStateAttribute.ToolCrafted, false);
        }

        public WorldState(Dictionary<WorldStateAttribute, object> states)
        {
            States = states;
        }

        public void AddState(WorldStateAttribute attr, object value)
        {
            if (value.GetType().IsPrimitive)
            {
                States.Add(attr, Convert.ToInt32(value));
            }
            else
            {
                States.Add(attr, value);
            }
        }

        public object RemoveState(WorldStateAttribute attr)
        {
            return States.Remove(attr);
        }

        public object UpdateState(WorldStateAttribute attr, object newValue)
        {
            if (!States.ContainsKey(attr)) return false;

            States[attr] = newValue;
            return true;
        }

        public object AddOrUpdateState(WorldStateAttribute attr, object newValue)
        {
            if (!States.ContainsKey(attr))
            {
                AddState(attr, newValue);
            }

            States[attr] = newValue;
            return true;
        }

        public bool CheckState(WorldStateAttribute attr, object value)
        {
            if (!States.ContainsKey(attr)) return false;

            return States[attr].ToString() == value.ToString(); // kinda hacky, but was the workaround I found to the issue that comparing equal ints was always failing
        }

        public object GetStateValue(WorldStateAttribute attr)
        {
            if (!States.ContainsKey(attr)) return null; 

            return States[attr];
        }

        public int GetStateCount()
        {
            return States.Count;
        }

        public static WorldState operator+(WorldState ws1, WorldState ws2)
        {
            return new WorldState(ws1.States.Concat(ws2.States).ToDictionary(x => x.Key, x => x.Value));
        }
    }
}

