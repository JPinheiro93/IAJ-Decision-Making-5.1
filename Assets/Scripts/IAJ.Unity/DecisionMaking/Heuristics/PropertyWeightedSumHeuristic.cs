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
            int maxHP = (int)state.GetProperty(Properties.MAXHP);
            int maxMana = 10;
            float maxTime = 200.0f;

            //Score
            float money = -(int)state.GetProperty(Properties.MONEY);
            float HP = maxHP - (int)state.GetProperty(Properties.HP);
            float XP = -(int)state.GetProperty(Properties.XP);
            float mana = maxMana - (int)state.GetProperty(Properties.MANA);
            float time = (float)state.GetProperty(Properties.TIME);

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
