﻿using Assets.Scripts.GameManager;
using System.Collections.Generic;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTSNode
    {
        public WorldModel State { get; private set; }
        public MCTSNode Parent { get; set; }
        public GOB.Action Action { get; set; }
        public int PlayerID { get; set; }
        public List<MCTSNode> ChildNodes { get; private set; }
        public int N { get; set; }
        public float Q { get; set; }

        public MCTSNode(WorldModel state)
        {
            this.State = state;
            this.ChildNodes = new List<MCTSNode>();
            this.N = 0;
            this.Q = 0;
        }
    }
}
