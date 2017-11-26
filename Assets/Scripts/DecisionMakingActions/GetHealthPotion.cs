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
            return base.CanExecute() && this.Character.GameManager.characterData.HP < this.Character.GameManager.characterData.MaxHP;
        }

		public override bool CanExecute(WorldModel worldModel)
		{
            return base.CanExecute() && (int)worldModel.GetProperty(Properties.HP) < (int)worldModel.GetProperty(Properties.MAXHP);
        }

		public override void Execute()
		{
            base.Execute();
            this.Character.GameManager.GetHealthPotion(this.Target);
        }

		public override void ApplyActionEffects(WorldModel worldModel)
		{
            base.ApplyActionEffects(worldModel);

            var maxHP = (int)worldModel.GetProperty(Properties.MAXHP);

            worldModel.SetProperty(Properties.HP, maxHP);
            //disables the target object so that it can't be reused again
            worldModel.SetProperty(this.Target.name, false);
        }
    }
}
