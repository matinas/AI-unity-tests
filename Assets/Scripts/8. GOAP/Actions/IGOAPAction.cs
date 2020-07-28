using System;
using System.Collections.Generic;

namespace AITests.GOAP.Actions
{
    public interface IGOAPAction
    {
        event Action OnActionCompleted;

        Dictionary<WorldStateAttribute, object> Preconditions { get; set; }
        Dictionary<WorldStateAttribute, object> Effect { get; set; }
    }
}