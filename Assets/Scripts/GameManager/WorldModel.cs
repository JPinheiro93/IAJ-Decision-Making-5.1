using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.GOB.Action;

namespace Assets.Scripts.GameManager
{
    public abstract class WorldModel
    {
        protected const int PropertiesArraySize = 25;
        protected object[] PropertiesArray { get; set; }
        private List<Action> Actions { get; set; }
        private List<Action> ExcecutableActions { get; set; }
        protected IEnumerator<Action> ActionEnumerator { get; set; } 

        //TODO: GoalValues took off because of MCTS. Add them back for DLGOAP.
        //private Dictionary<string, float> GoalValues { get; set; } 

        public WorldModel(List<Action> actions)
        {
            this.PropertiesArray = new object[PropertiesArraySize];
            //this.GoalValues = new Dictionary<string, float>();
            this.Actions = actions;
            this.ActionEnumerator = actions.GetEnumerator();
        }

        public WorldModel(WorldModel parent)
        {
            this.PropertiesArray = parent.PropertiesArray.ToArray();
            //this.GoalValues = new Dictionary<string, float>();
            this.Actions = parent.Actions;
            this.ActionEnumerator = this.Actions.GetEnumerator();
        }

        public virtual object GetProperty(string propertyName)
        {
            var propertyIndex = Properties.GetPropertyIndex(propertyName);
            return this.GetPropertyByIndex(propertyIndex);
        }

        protected virtual object GetPropertyByIndex(int index)
        {
            return this.PropertiesArray[index];
        }

        public virtual void SetProperty(string propertyName, object value)
        {
            var propertyIndex = Properties.GetPropertyIndex(propertyName);
            this.PropertiesArray[propertyIndex] = value;
        }

        public virtual float GetGoalValue(string goalName)
        {
            ////recursive implementation of WorldModel
            //if (this.GoalValues.ContainsKey(goalName))
            //{
            //    return this.GoalValues[goalName];
            //}
            //else if (this.Parent != null)
            //{
            //    return this.Parent.GetGoalValue(goalName);
            //}
            //else
            //{
            //    return 0;
            //}
            return 0;
        }

        public virtual void SetGoalValue(string goalName, float value)
        {
            //var limitedValue = value;
            //if (value > 10.0f)
            //{
            //    limitedValue = 10.0f;
            //}

            //else if (value < 0.0f)
            //{
            //    limitedValue = 0.0f;
            //}

            //this.GoalValues[goalName] = limitedValue;
        }

        public abstract WorldModel GenerateChildWorldModel(Action action);

        public float CalculateDiscontentment(List<Goal> goals)
        {
            var discontentment = 0.0f;
            
            foreach (var goal in goals)
            {
                var newValue = this.GetGoalValue(goal.Name);

                discontentment += goal.GetDiscontentment(newValue);
            }

            return discontentment;
        }

        public virtual Action GetNextAction()
        {
            Action action = null;
            //returns the next action that can be executed or null if no more executable actions exist
            if (this.ActionEnumerator.MoveNext())
            {
                action = this.ActionEnumerator.Current;
            }

            while (action != null && !action.CanExecute(this))
            {
                if (this.ActionEnumerator.MoveNext())
                {
                    action = this.ActionEnumerator.Current;    
                }
                else
                {
                    action = null;
                }
            }

            return action;
        }

        public virtual List<Action> GetExecutableActions()
        {
            return this.Actions.Where(a => a.CanExecute(this)).ToList();
        }

        public abstract bool IsTerminal();

        public abstract float GetScore();

        public abstract int GetNextPlayer();

        public abstract void CalculateNextPlayer();
    }
}
