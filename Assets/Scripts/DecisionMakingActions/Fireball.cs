﻿using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using UnityEngine;
using System;

namespace Assets.Scripts.DecisionMakingActions
{
    public class Fireball : WalkToTargetAndExecuteAction
    {
        private int xpChange;

		public Fireball(AutonomousCharacter character, GameObject target) : base("Fireball",character,target)
		{
            if (target.tag.Equals("Skeleton"))
            {
                this.xpChange = 5;
            }
            else if (target.tag.Equals("Orc"))
            {
                this.xpChange = 10;
            }
            else if (target.tag.Equals("Dragon"))
            {
            }
        }

		public override float GetGoalChange(Goal goal)
		{
            var change = base.GetGoalChange(goal);

            if (goal.Name == AutonomousCharacter.GAIN_XP_GOAL)
            {
                change += -this.xpChange;
            }

            return change;
        }

        public override bool CanExecute()
        {
            return base.CanExecute() && this.Character.GameManager.characterData.Mana >= 5;
        }

        public override bool CanExecute(WorldModel worldModel)
        {
            return base.CanExecute() && (int)worldModel.GetProperty(Properties.MANA) >= 5;
        }

        public override void Execute()
		{
            base.Execute();
            this.Character.GameManager.Fireball(this.Target);
        }


		public override void ApplyActionEffects(WorldModel worldModel)
		{
            base.ApplyActionEffects(worldModel);

            if (!this.Target.tag.Equals("Dragon"))
            {
                var xpValue = worldModel.GetGoalValue(AutonomousCharacter.GAIN_XP_GOAL);
                worldModel.SetGoalValue(AutonomousCharacter.GAIN_XP_GOAL, xpValue - this.xpChange);

                var xp = (int)worldModel.GetProperty(Properties.XP);
                worldModel.SetProperty(Properties.XP, xp + this.xpChange);

                //disables the target object so that it can't be reused again
                worldModel.SetProperty(this.Target.name, false);
            }
        }

    }
}
