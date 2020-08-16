using System;
using System.Collections.Generic;
using System.Linq;

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
    public class WorldStatePair // WorldState pair to be used for fields that needs to be filled through Inspector (assumes integer values)
    {
        public WorldStateAttribute Attr;
        public int Value;

        public WorldStatePair(WorldStateAttribute attr, int value)
        {
            Attr = attr;
            Value = value;
        }
    }

    public struct WorldStateKeyPair // WorldState pair to be used for representing single states of a WorldState through the code base (keeps object values)
    {
        public WorldStateAttribute Attr { get; private set; }
        public object Value { get; private set; }

        public WorldStateKeyPair(WorldStateAttribute attr, object value)
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
        }

        public WorldState(Dictionary<WorldStateAttribute, object> states)
        {
            States = states;
        }

        public WorldState(WorldState worldState)
        {
            States = worldState.States.ToDictionary(x => x.Key, x=> x.Value);
        }

        public void AddState(WorldStateAttribute attr, object value)
        {
            States.Add(attr, NormalizeValue(value));
        }

        // to unify the way we handle world state values, we are assuming they'll get assigned either
        // a boolean or an integer (in which case the value will be converted/normalized to an integer),
        // or a predicate function (case of Action's procedural preconditions, in which case it will be left as it is)
        public static object NormalizeValue(object value)
        {
            return value.GetType().IsPrimitive ? Convert.ToInt32(value) : value;
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
            else
            {
                States[attr] = newValue;
            }
            
            return true;
        }

        public bool CheckStateValue(WorldStateAttribute attr, object value)
        {
            if (!States.ContainsKey(attr)) return false;

            return System.Object.Equals(States[attr], value); // doesn't work with == as it checks whether the memory locations of the objects are the same, while Equals() checks content/values
        }

        public object GetStateValue(WorldStateAttribute attr)
        {
            if (!States.ContainsKey(attr)) return null; 

            return States[attr];
        }

        public static WorldState operator+(WorldState ws1, WorldState ws2)
        {
            return new WorldState(ws1.States.Concat(ws2.States).ToDictionary(x => x.Key, x => x.Value));
        }
    }
}

