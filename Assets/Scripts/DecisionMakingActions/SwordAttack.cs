﻿using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using UnityEngine;

namespace Assets.Scripts.DecisionMakingActions
{
    public class SwordAttack : WalkToTargetAndExecuteAction
    {
        private int hpChange;
        private int xpChange;

        public SwordAttack(AutonomousCharacter character, GameObject target) : base("SwordAttack",character,target)
        {
           
            if (target.tag.Equals("Skeleton"))
            {
                this.hpChange = -5;
                this.xpChange = 5;
            }
            else if (target.tag.Equals("Orc"))
            {
                this.hpChange = -10;
                this.xpChange = 10;
            }
            else if (target.tag.Equals("Dragon"))
            {
                this.hpChange = -20;
                this.xpChange = 20;
            }
        }

        public override float GetGoalChange(Goal goal)
        {
            var change = base.GetGoalChange(goal);

            if (goal.Name == AutonomousCharacter.SURVIVE_GOAL)
            {
                change += -this.hpChange;
            }
            else if (goal.Name == AutonomousCharacter.GAIN_XP_GOAL)
            {
                change += -this.xpChange;
            }
            
            return change;
        }


        public override void Execute()
        {
            base.Execute();
            this.Character.GameManager.SwordAttack(this.Target);
        }

        public override void ApplyActionEffects(WorldModel worldModel)
        {
            base.ApplyActionEffects(worldModel);

            var xpValue = worldModel.GetGoalValue(AutonomousCharacter.GAIN_XP_GOAL);
            worldModel.SetGoalValue(AutonomousCharacter.GAIN_XP_GOAL,xpValue-this.xpChange); 

            var hp = (int)worldModel.GetProperty(Properties.HP);
            worldModel.SetProperty(Properties.HP,hp + this.hpChange);

            if ((int)worldModel.GetProperty(Properties.HP) > 0)
            {
                var xp = (int)worldModel.GetProperty(Properties.XP);
                worldModel.SetProperty(Properties.XP, xp + this.xpChange);

                var surviveValue = worldModel.GetGoalValue(AutonomousCharacter.SURVIVE_GOAL);
                worldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL, surviveValue - this.hpChange);

                //disables the target object so that it can't be reused again
                worldModel.SetProperty(this.Target.name, false);
            }
        }

    }
}
