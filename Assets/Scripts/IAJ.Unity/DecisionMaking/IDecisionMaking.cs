using System.Collections.Generic;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking
{
    public interface IDecisionMaking
    {
        bool InProgress { get; set; }
        float TotalProcessingTime { get; set; }
        GOB.Action BestAction { get; set; }
        List<GOB.Action> BestActionSequence { get; set; }

        void InitializeDecisionMaking();
        GOB.Action ChooseAction();
    }
}
