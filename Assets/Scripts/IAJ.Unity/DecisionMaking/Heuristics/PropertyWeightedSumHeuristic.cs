using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using RAIN.Navigation.Graph;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.Heuristics
{
    public class PropertyWeightedSumHeuristic : IHeuristic
    {
        public float H(WorldModel state)
        {
            //Max Possible scores
            int maxHP = state.GetProperty<int>(Properties.MAXHP);
            int maxMana = 10;
            float maxTime = 200.0f;

            //Score
            float money = state.GetProperty<int>(Properties.MONEY);
            float HP = maxHP - state.GetProperty<int>(Properties.HP);
            float XP = state.GetProperty<int>(Properties.XP);
            float mana = maxMana - state.GetProperty<int>(Properties.MANA);
            float time = state.GetProperty<float>(Properties.TIME);

            return (HP / maxHP + time / maxTime + money) / 3 * (XP + mana);
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
