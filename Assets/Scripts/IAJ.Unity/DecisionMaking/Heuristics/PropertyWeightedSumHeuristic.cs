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
            float time = (float)state.GetProperty(Properties.TIME);
            int hp = (int)state.GetProperty(Properties.HP);
            int level = (int)state.GetProperty(Properties.LEVEL);

            //Lose
            if (hp <= 0 || time >= 200)
            {
                return 0;
            }
            //Win
            else if (money == 25)
            {
                return 1.0f;
            }
            //Score
            var result = (money * 6 / 25 + (200 - time) / 200 + level * 3 / 3) / 10;

            return result;
        }

        public float H(WorldModel state, Action action)
        {
            return H(state.GenerateChildWorldModel(action));
        }
    }
}
