using Assets.Scripts.GameManager;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.GOB
{
    public class DepthLimitedGOAPDecisionMaking
    {
        public const int MAX_DEPTH = 3;
        public int ActionCombinationsProcessedPerFrame { get; set; }
        public float TotalProcessingTime { get; set; }
        public int TotalActionCombinationsProcessed { get; set; }
        public bool InProgress { get; set; }

        public CurrentStateWorldModel InitialWorldModel { get; set; }
        private List<Goal> Goals { get; set; }
        private WorldModel[] Models { get; set; }
        private Action[] ActionPerLevel { get; set; }
        public Action[] BestActionSequence { get; private set; }
        public Action BestAction { get; private set; }
        public float BestDiscontentmentValue { get; private set; }
        private int CurrentDepth { get; set; }

        public DepthLimitedGOAPDecisionMaking(CurrentStateWorldModel currentStateWorldModel, List<Action> actions, List<Goal> goals)
        {
            this.ActionCombinationsProcessedPerFrame = 200;
            this.Goals = goals;
            this.InitialWorldModel = currentStateWorldModel;
        }

        public void InitializeDecisionMakingProcess()
        {
            this.InProgress = true;
            this.TotalProcessingTime = 0.0f;
            this.TotalActionCombinationsProcessed = 0;
            this.CurrentDepth = 0;
            this.Models = new WorldModel[MAX_DEPTH + 1];
            this.Models[0] = this.InitialWorldModel;
            this.ActionPerLevel = new Action[MAX_DEPTH];
            this.BestActionSequence = new Action[MAX_DEPTH];
            this.BestAction = null;
            this.BestDiscontentmentValue = float.MaxValue;
            this.InitialWorldModel.Initialize();
        }

        public Action ChooseAction()
        {
			var processedActions = 0;
			var startTime = Time.realtimeSinceStartup;
            var initialState = this.InitialWorldModel.GenerateChildWorldModel();

            //TODO: these values implement a random agent (obtains victory for lab5, though!!!)
            this.BestAction = initialState.GetNextAction();
            this.BestActionSequence[0] = initialState.GetNextAction();
            this.BestActionSequence[1] = initialState.GetNextAction();
            this.BestActionSequence[2] = initialState.GetNextAction();
            this.BestDiscontentmentValue = initialState.CalculateDiscontentment(this.Goals);

            //TODO: implement!
            //while (processedActions <= this.ActionCombinationsProcessedPerFrame)
            //{
            //    Models.
            //    foreach (var action in initialState.GetExecutableActions())
            //    {

            //    }
            //}

            this.TotalProcessingTime += Time.realtimeSinceStartup - startTime;
			this.InProgress = false;
			return this.BestAction;
        }
    }
}
