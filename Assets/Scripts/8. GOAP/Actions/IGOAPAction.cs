using System;
using System.Collections.Generic;

namespace AITests.GOAP.Actions
{
    public interface IGOAPAction
    {
        event Action OnActionCompleted;
        event Action OnActionAborted;

        WorldState Preconditions { get; set; }
        WorldState Effects { get; set; }

        void SetFixedPreconditions();
        void SetProceduralPreconditions();
        void SetFixedEffects();

        bool PreRun();
        bool Run();
        bool PostRun();
    }
}