using Assets.Scripts.GameManager;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.Heuristics
{
    public interface IHeuristic
    {
        float H(WorldModel state);

        float H(WorldModel state, GOB.Action action);
    }
}
