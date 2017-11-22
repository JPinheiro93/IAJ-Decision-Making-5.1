using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTS : IDecisionMaking
    {
        public const float C = 1.4142135623730950488016887242097f; // sqrt(2)
        public bool InProgress { get; set; }
        public int MaxIterations { get; set; }
        public int MaxIterationsProcessedPerFrame { get; set; }
        public int MaxPlayoutDepthReached { get; private set; }
        public int MaxSelectionDepthReached { get; private set; }
        public float TotalProcessingTime { get; set; }
        public GOB.Action BestAction { get; set; }
        public MCTSNode BestFirstChild { get; set; }
        public List<GOB.Action> BestActionSequence { get; set; }

        private int CurrentIterations { get; set; }
        private int CurrentIterationsInFrame { get; set; }
        private int CurrentDepth { get; set; }

        private CurrentStateWorldModel CurrentStateWorldModel { get; set; }
        private MCTSNode InitialNode { get; set; }
        private System.Random RandomGenerator { get; set; }
        
        public MCTS(CurrentStateWorldModel currentStateWorldModel)
        {
            this.InProgress = false;
            this.CurrentStateWorldModel = currentStateWorldModel;
            this.MaxIterations = 100;
            this.MaxIterationsProcessedPerFrame = 10;
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

            var currentNode = selectedNode;
            while (currentNode != null && currentNode.Action != null)
            {
                this.BestActionSequence.Add(currentNode.Action);
                currentNode = currentNode.Parent;
            }

            this.BestAction = selectedNode.Action;

            return this.BestAction;
        }

        private MCTSNode Selection(MCTSNode initialNode)
        {
            GOB.Action nextAction;
            MCTSNode currentNode = initialNode;
            //MCTSNode bestChild;

            var currentSelectionDepth = 0;
            
            //TODO: bestChild is for what?
            //TODO: should we really return after expansion? Why?
            while (!currentNode.State.IsTerminal())
            {
                nextAction = currentNode.State.GetNextAction();
                if (nextAction != null)
                {
                    return this.Expand(currentNode, nextAction);
                }
                else 
                {
                    currentNode = this.BestChild(currentNode);
                    currentSelectionDepth++;
                }
            }

            if (currentSelectionDepth > this.MaxSelectionDepthReached)
            {
                this.MaxSelectionDepthReached = currentSelectionDepth;
            }

            return currentNode;
        }

        private Reward Playout(WorldModel initialPlayoutState)
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
                //TODO: score is always 0. Should use heuristic? Or should add goals to MCTS and use Discontentment?
                Value = currentState.GetScore()
            };
        }

        private void Backpropagate(MCTSNode node, Reward reward)
        {
            var currentNode = node;

            //TODO: remodel to multiplayer in lab 7
            while (currentNode != null)
            {
                currentNode.N++;
                currentNode.Q += reward.Value;
                currentNode = currentNode.Parent;
            }
        }
        
        private MCTSNode Expand(MCTSNode parent, GOB.Action action)
        {
            var childState = parent.State.GenerateChildWorldModel();
            action.ApplyActionEffects(childState);
            childState.CalculateNextPlayer();

            //TODO: validate if parent is altered after function ends.
            var childNode = new MCTSNode(childState) { Parent = parent, Action = action };
            parent.ChildNodes.Add(childNode);

            return childNode;
        }

        //gets the best child of a node, using the UCT formula
        private MCTSNode BestUCTChild(MCTSNode node)
        {
            //TODO: implement
            throw new NotImplementedException();
        }

        //this method is very similar to the bestUCTChild, but it is used to return the final action of the MCTS search, and so we do not care about
        //the exploration factor
        private MCTSNode BestChild(MCTSNode node)
        {
            //TODO: implement
            throw new NotImplementedException();
        }
    }
}
