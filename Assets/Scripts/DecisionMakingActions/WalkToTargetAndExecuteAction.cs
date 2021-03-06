﻿using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using System;
using UnityEngine;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.GOB.Action;

namespace Assets.Scripts.DecisionMakingActions
{
    //TODO: Optimization2: Use gateway Heuristic to measure distance. FAILED: Too complex to implement in this project.
    public abstract class WalkToTargetAndExecuteAction : Action
    {
        protected AutonomousCharacter Character { get; set; }
        protected GameObject Target { get; set; }
        private const float SquareRoot2 = 1.4142135623730950488016887242097f; // sqrt(2)

        protected WalkToTargetAndExecuteAction(string actionName, AutonomousCharacter character, GameObject target) : base(actionName + "(" + target.name + ")")
        {
            this.Character = character;
            this.Target = target;
            //TODO: Load gateways and hypergraph.
        }

        public override float GetDuration()
        {
            return this.GetDuration(this.Character.Character.KinematicData.position);
        }

        public override float GetDuration(WorldModel worldModel)
        {
            var position = (Vector3)worldModel.GetProperty(Properties.POSITION);
            return this.GetDuration(position);
        }

        private float GetDuration(Vector3 currentPosition)
        {
            var targetPosition = this.Target.transform.position;
            var zDistance = targetPosition.z - currentPosition.z;
            var xDistance = targetPosition.x - currentPosition.x;

            //TODO: include on report that we should use gateway heuristic. this one is not good for closed maps. (lab 5 report only, no need to implement)
            var distance =  Math.Max(zDistance, xDistance) + (SquareRoot2 - 1) * Math.Min(zDistance, xDistance);              //OCTILE DISTANCE
                            //(this.Target.transform.position - this.Character.Character.KinematicData.position).magnitude;     //EUCLIDEAN DISTANCE
                            //zDistance + xDistance;                                                                              //MANHATTAN DISTANCE
            return distance / this.Character.Character.MaxSpeed;
        }

        public override float GetGoalChange(Goal goal)
        {
            if (goal.Name == AutonomousCharacter.BE_QUICK_GOAL)
            {
                return this.GetDuration();
            }
            else return 0;
        }

        public override bool CanExecute()
        {
            return this.Target != null;
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            if (this.Target == null) return false;
            var targetEnabled = (bool)worldModel.GetProperty(this.Target.name);
            return targetEnabled;
        }

        public override void Execute()
        {
            this.Character.StartPathfinding(this.Target.transform.position);
        }

        public override void ApplyActionEffects(WorldModel worldModel)
        {
            var duration = this.GetDuration(worldModel);

            var quicknessValue = worldModel.GetGoalValue(AutonomousCharacter.BE_QUICK_GOAL);
            worldModel.SetGoalValue(AutonomousCharacter.BE_QUICK_GOAL, quicknessValue + duration * 0.1f);

            var time = (float)worldModel.GetProperty(Properties.TIME);
            worldModel.SetProperty(Properties.TIME, time + duration);

            worldModel.SetProperty(Properties.POSITION, Target.transform.position);
        }
    }
}