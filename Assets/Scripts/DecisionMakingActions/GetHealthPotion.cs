using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using System;
using UnityEngine;

namespace Assets.Scripts.DecisionMakingActions
{
    public class GetHealthPotion : WalkToTargetAndExecuteAction
    {
        public GetHealthPotion(AutonomousCharacter character, GameObject target) : base("GetHealthPotion",character,target)
        {
        }

		public override bool CanExecute()
		{
            return base.CanExecute() && this.Character.GameManager.characterData.HP < 10;
        }

		public override bool CanExecute(WorldModel worldModel)
		{
            return base.CanExecute() && worldModel.GetProperty<int>(Properties.HP) < 10;
        }

		public override void Execute()
		{
            base.Execute();
            this.Character.GameManager.GetHealthPotion(this.Target);
        }

		public override void ApplyActionEffects(WorldModel worldModel)
		{
            base.ApplyActionEffects(worldModel);

            var maxHP = worldModel.GetProperty<int>(Properties.MAXHP);

            worldModel.SetProperty(Properties.HP, maxHP);
            //disables the target object so that it can't be reused again
            worldModel.SetProperty(this.Target.name, false);
        }
    }
}
