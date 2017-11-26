using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using RAIN.Navigation.Graph;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.Heuristics
{
    public class PropertyWeightedSumHeuristic : IHeuristic
    {
        public float H(WorldModel state)
        {
            int money = (int)state.GetProperty(Properties.MONEY);
            float timeLeft = 200 - (float)state.GetProperty(Properties.TIME);
            int hp = (int)state.GetProperty(Properties.HP);
            int level = (int)state.GetProperty(Properties.LEVEL);

            //Lose
            if (hp <= 0 || timeLeft <= 0)
            {
                return 0;
            }
            //Win
            else if (money >= 25)
            {
                return 1.0f + (timeLeft / 200);
            }
            //Score
            var result = ((money + 1) * (9/10) + level * (1/10)) * (timeLeft / 200);

            return result;
        }

        public float H(WorldModel state, Action action)
        {
            return H(state.GenerateChildWorldModel(action));
        }
    }
}
