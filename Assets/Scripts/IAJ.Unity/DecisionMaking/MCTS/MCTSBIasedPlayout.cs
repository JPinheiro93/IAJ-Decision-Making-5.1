﻿using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using Assets.Scripts.IAJ.Unity.DecisionMaking.Heuristics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTSBiasedPlayout : MCTS
    {
        private IHeuristic Heuristic { get;  set; }
        
        public MCTSBiasedPlayout(CurrentStateWorldModel currentStateWorldModel, IHeuristic heuristic)
            : base(currentStateWorldModel)
        {
            this.Heuristic = heuristic;
        }

        protected override float RunPlayout(WorldModel initialPlayoutState)
        {
            GOB.Action nextAction;
            WorldModel currentState = initialPlayoutState;
            var currentPlayoutDepth = 0;

            while (!currentState.IsTerminal())
            {
                var executableActions = currentState.GetExecutableActions();

                //Bias: Choose among the 50% best
                var maxIndex = Convert.ToInt32(Math.Ceiling(executableActions.Count * 0.5));
                var orderedExecutableActions = executableActions.OrderByDescending(x => this.Heuristic.H(currentState, x)).Take(maxIndex).ToList();

                var index = this.RandomGenerator.Next(0, maxIndex);
                nextAction = executableActions[index];

                currentState = currentState.GenerateChildWorldModel(nextAction);

                currentPlayoutDepth++;
            }

            if (currentPlayoutDepth > this.MaxPlayoutDepthReached)
            {
                this.MaxPlayoutDepthReached = currentPlayoutDepth;
            }

            //var currentPlayer = currentState.GetNextPlayer();
            //var value = initialPlayoutState.GetNextPlayer() == currentPlayer ? score : -score;

            return currentState.GetScore();
        }
    }
}
