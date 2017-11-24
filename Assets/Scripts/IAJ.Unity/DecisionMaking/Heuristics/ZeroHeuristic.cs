using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using RAIN.Navigation.Graph;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.Heuristics
{
    public class ZeroHeuristic : IHeuristic
    {
        public float H(WorldModel state)
        {
            return 0;
        }

        public float H(WorldModel state, Action action)
        {
            var childState = state.GenerateChildWorldModel();
            action.ApplyActionEffects(childState);
            childState.CalculateNextPlayer();

            return H(childState);
        }
    }
}
