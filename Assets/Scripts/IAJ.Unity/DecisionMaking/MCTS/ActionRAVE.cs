using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class ActionRAVE
    {
        public readonly GOB.Action action;
        public float Q { get; private set; }
        public int Plays { get; private set; }

        public ActionRAVE(GOB.Action action)
        {
            this.action = action;
            this.Q = 0;
            this.Plays = 0;
        }

        public void Update(float rewardValue)
        {
            this.Plays++;
            this.Q += rewardValue;
        }
    }
}
