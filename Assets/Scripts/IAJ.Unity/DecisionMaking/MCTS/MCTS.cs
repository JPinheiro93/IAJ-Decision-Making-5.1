using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using Assets.Scripts.IAJ.Unity.DecisionMaking.Heuristics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTS : IDecisionMaking
    {
        public const float ExplorationFactor = 1.4142135623730950488016887242097f; // sqrt(2)
        public bool InProgress { get; set; }
        public int MaxIterations { get; set; }
        public int MaxIterationsProcessedPerFrame { get; set; }
        public int MaxPlayoutDepthReached { get; protected set; }
        public int MaxSelectionDepthReached { get; protected set; }
        public float TotalProcessingTime { get; set; }
        public GOB.Action BestAction { get; set; }
        public MCTSNode BestFirstChild { get; set; }
        public List<GOB.Action> BestActionSequence { get; set; }

        protected int CurrentIterations { get; set; }
        protected int CurrentIterationsInFrame { get; set; }
        protected int CurrentDepth { get; set; }

        protected CurrentStateWorldModel CurrentStateWorldModel { get; set; }
        protected MCTSNode InitialNode { get; set; }
        protected System.Random RandomGenerator { get; set; }
        
        public MCTS(CurrentStateWorldModel currentStateWorldModel)
        {
            this.InProgress = false;
            this.CurrentStateWorldModel = currentStateWorldModel;
            this.MaxIterations = 5000;
            this.MaxIterationsProcessedPerFrame = 100;
            this.RandomGenerator = new System.Random();
        }

        public void InitializeDecisionMaking()
        {
            this.MaxPlayoutDepthReached = 0;
            this.MaxSelectionDepthReached = 0;
            this.CurrentIterations = 0;
            this.CurrentIterationsInFrame = 0;
            this.TotalProcessingTime = 0.0f;
            this.CurrentStateWorldModel.Initialize();
            this.InitialNode = new MCTSNode(this.CurrentStateWorldModel)
            {
                Action = null,
                Parent = null,
                PlayerID = 0
            };
            this.InProgress = true;
            this.BestFirstChild = null;
            this.BestActionSequence = new List<GOB.Action>();
        }

        public GOB.Action ChooseAction()
        {
            MCTSNode selectedNode = this.InitialNode;
            Reward reward;
            MCTSNode bestChild;

            var startTime = Time.realtimeSinceStartup;
            this.CurrentIterationsInFrame = 0;
            
            while (this.CurrentIterationsInFrame < this.MaxIterationsProcessedPerFrame 
                && this.CurrentIterations < this.MaxIterations)
            {
                selectedNode = this.Selection(this.InitialNode);
                reward = this.Playout(selectedNode.State);
                this.Backpropagate(selectedNode, reward);

                this.CurrentIterations++;
                this.CurrentIterationsInFrame++;
            }

            if (this.CurrentIterationsInFrame < this.MaxIterationsProcessedPerFrame)
            {
                if (!this.InitialNode.State.IsTerminal())
                {
                    bestChild = this.BestChild(this.InitialNode);

                    var currentNode = bestChild;
                    while (currentNode != null && currentNode.Action != null)
                    {
                        this.BestActionSequence.Add(currentNode.Action);
                        currentNode = currentNode.Parent;
                    }

                    this.BestAction = bestChild.Action;
                }
                
                this.InProgress = false;
            }

            this.TotalProcessingTime += Time.realtimeSinceStartup - startTime;
            return this.BestAction;
        }

        private MCTSNode Selection(MCTSNode initialNode)
        {
            GOB.Action nextAction;
            MCTSNode currentNode = initialNode;

            var currentSelectionDepth = 0;
        
            while (!currentNode.State.IsTerminal())
            {
                nextAction = currentNode.State.GetNextAction();
                if (nextAction != null)
                {
                    return this.Expand(currentNode, nextAction);
                }
                else 
                {
                    currentNode = this.BestUCTChild(currentNode);
                    currentSelectionDepth++;
                }
            }

            if (currentSelectionDepth > this.MaxSelectionDepthReached)
            {
                this.MaxSelectionDepthReached = currentSelectionDepth;
            }

            return currentNode;
        }

        protected virtual Reward Playout(WorldModel initialPlayoutState)
        {
            GOB.Action nextAction;
            WorldModel currentState = initialPlayoutState;
            var currentPlayoutDepth = 0;

            while (!currentState.IsTerminal())
            {
                var executableActions = currentState.GetExecutableActions();
                nextAction = executableActions[this.RandomGenerator.Next(0, executableActions.Length)];

                currentState = currentState.GenerateChildWorldModel();
                nextAction.ApplyActionEffects(currentState);
                currentState.CalculateNextPlayer();

                currentPlayoutDepth++;
            }

            if (currentPlayoutDepth > this.MaxPlayoutDepthReached)
            {
                this.MaxPlayoutDepthReached = currentPlayoutDepth;
            }

            return new Reward
            {
                PlayerID = currentState.GetNextPlayer(),
                Value = currentState.GetScore()
            };
        }

        private void Backpropagate(MCTSNode node, Reward reward)
        {
            var currentNode = node;
            
            while (currentNode != null)
            {
                currentNode.N++;
                currentNode.Q += (this.InitialNode.PlayerID == currentNode.PlayerID) ? reward.Value : -reward.Value;
                currentNode = currentNode.Parent;
            }
        }
        
        private MCTSNode Expand(MCTSNode parent, GOB.Action action)
        {
            var childState = parent.State.GenerateChildWorldModel();
            action.ApplyActionEffects(childState);
            childState.CalculateNextPlayer();
            
            var childNode = new MCTSNode(childState) { Parent = parent, Action = action, PlayerID = childState.GetNextPlayer() };
            parent.ChildNodes.Add(childNode);

            return childNode;
        }

        //gets the best child of a node, using the UCT formula
        private MCTSNode BestUCTChild(MCTSNode node)
        {
            return node.ChildNodes.OrderByDescending(x => (x.Q / x.N) + ExplorationFactor * Math.Sqrt(Math.Log(node.N) / x.N)).First();
        }

        //this method is very similar to the bestUCTChild, but it is used to return the final action of the MCTS search, and so we do not care about
        //the exploration factor
        private MCTSNode BestChild(MCTSNode node)
        {
            return node.ChildNodes.OrderByDescending(x => x.Q / x.N).First();
        }
    }
}
